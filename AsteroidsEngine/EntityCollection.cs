using System.Collections.Generic;
using System.Linq;
using System;
using OpenTK;

namespace AsteroidsEngine
{
    public class EntityCollection
    {
        private readonly List<ActiveList> _collection;
        private readonly Dictionary<string, ICollider> _colliders;
        private readonly Dictionary<string, UpdateComponent> _components;
        private readonly List<RenderComponent> _renders;
        private readonly Shader _shader;
        private readonly Engine _engine;


        public EntityCollection(Shader shader, Engine engine)
        {
            var tagsNumber = Enum.GetNames(typeof(Tags)).Length;
            _collection = new List<ActiveList>(tagsNumber);
            for (var i = 0; i < tagsNumber; i++)
            {
                _collection.Add(new ActiveList());
            }

            _components = new Dictionary<string, UpdateComponent>();
            _colliders = new Dictionary<string, ICollider>();
            _renders = new List<RenderComponent>();

            _shader = shader;
            _engine = engine;
        }

        public void FillColliders()
        {
            AddCollider("bullet", new BulletCollider());
            AddCollider("asteroid", new AsteroidCollider(_engine.Variables, this));
            AddCollider("ship", new ShipCollider(_engine));
            AddCollider("ufo", new UfoCollider(_engine.Variables));
        }

        public void AddRender(RenderComponent renderComponent)
        {
            _renders.Add(renderComponent);
        }

        public void FillComponents()
        {
            AddComponent("hyper", new HyperDriveComponent());
            AddComponent("decay", new DecayComponent());
            AddComponent("player", new PlayerComponent(_engine.Variables, _engine.CurController, this));
            AddComponent("spawner", new AsteroidSpawnerComponent(this));
            AddComponent("ufo_ai", new UfoAiComponent(this));
            AddComponent("laser_charge", new LaserChargeComponent(_engine.Variables));
            AddComponent("score", new ScoreDigitComponent(_engine.Variables, this));
            AddComponent("game_over", new GameOverComponent(_engine.Variables));
        }

        private void AddComponent(string name, UpdateComponent component)
        {
            _components.Add(name, component);
        }

        private UpdateComponent GetComponent(string name)
        {
            return _components[name];
        }

        private void AddCollider(string name, ICollider collider)
        {
            _colliders.Add(name, collider);
        }

        private ICollider GetCollider(string name)
        {
            return _colliders[name];
        }

        private Entity ReuseOrCreate(Tags tag, int render)
        {
            var result = ReuseOrCreate(tag);

            result.SetRender(_renders[render]);

            return result;
        }

        private Entity ReuseOrCreate(Tags tag)
        {
            var result = _collection[(int) tag].GetLast();
            if (result != null) return result;

            result = new Entity(_shader) {Tag = tag};
            _collection[(int) tag].Add(result);
            return result;
        }

        public void Update(float delta)
        {
            foreach (var tag in _collection)
            foreach (var entity in tag)
                if (!entity.Update(delta))
                    tag.DeactivateCurrent();
        }

        public void Render()
        {
            foreach (var tag in _collection)
            foreach (var entity in tag)
                entity.Render();
        }

        public void Collide(Tags tag1, Tags tag2)
        {
            foreach (var entity1 in _collection[(int) tag1])
            {
                var cachePos = entity1.Position;
                var cacheSize = entity1.Scale * 2;
                foreach (var entity2 in _collection[(int) tag2])
                    if (Vector2.DistanceSquared(cachePos, entity2.Position) <
                        (cacheSize + entity2.Scale * 2) * (cacheSize + entity2.Scale * 2))
                    {
                        if (!entity1.Collide(entity2))
                            _collection[(int) tag1].DeactivateCurrent();    
                        if (!entity2.Collide(entity1))
                            _collection[(int) tag2].DeactivateCurrent();
                    }
            }
        }


        public void CleanUp()
        {
            Tags[] trash =
            {
                Tags.Asteroid, Tags.Bullet,
                Tags.Laser, Tags.Ufo
            };
            foreach (var tag in trash)
                _collection[(int) tag].DeactivateAll();
        }

        public Entity FindByTag(Tags tag)
        {
            return _collection[(int) tag].FirstOrDefault();
        }

        public RenderComponent GetRender(int num)
        {
            return _renders[num];
        }

        #region Factory Methods

        public void CreatePlayer()
        {
            var player = ReuseOrCreate(Tags.Player, 14);
            player.Position = Vector2.Zero;
            player.Velocity = Vector2.Zero;
            player.Rotation = 0.0f;
            if (player.ComponentsCount != 0) return;
            player.Scale = 0.025f;
            player.AddComponent(GetComponent("player"));
            player.AddComponent(GetComponent("hyper"));
            player.SetCollider(GetCollider("ship"));
        }

        public Entity CreateAsteroid()
        {
            var asteroid = ReuseOrCreate(Tags.Asteroid, 16);
            asteroid.Scale = 0.1f;
            asteroid.Position = -Vector2.UnitX * 0.8f;
            asteroid.Velocity = Vector2.One * 0.2f;
            if (asteroid.ComponentsCount != 0) return asteroid;

            asteroid.AddComponent(GetComponent("hyper"));
            asteroid.SetCollider(GetCollider("asteroid"));

            return asteroid;
        }

        public void CreateBullet(Entity origin)
        {
            var bullet = ReuseOrCreate(Tags.Bullet, 17);
            bullet.Timer = 0.5f;
            bullet.Scale = 0.005f;
            bullet.Position = origin.Position;
            bullet.Velocity = Vector2.Transform(Vector2.UnitY,
                Quaternion.FromAxisAngle(Vector3.UnitZ,
                    MathHelper.DegreesToRadians(origin.Rotation)));
            if (bullet.ComponentsCount > 0) return;
            bullet.AddComponent(GetComponent("decay"));
            bullet.AddComponent(GetComponent("hyper"));
            bullet.SetCollider(GetCollider("bullet"));
        }

        public void CreateBanner()
        {
            var banner = ReuseOrCreate(Tags.Ui, 13);
            banner.AddComponent(GetComponent("game_over"));
            banner.Active = true;
            banner.Scale = 0.25f;
        }

        public void CreateLaser(Entity origin)
        {
            var rot = origin.Rotation;
            var diff = Vector2.Transform(Vector2.UnitY * 0.02f,
                Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(rot)));

            var pos = origin.Position;
            while (Vector2.Clamp(pos, -Vector2.One, Vector2.One) == pos)

            {
                var laser = ReuseOrCreate(Tags.Laser, 11);
                laser.Position = pos;
                laser.Rotation = rot;
                laser.Scale = 0.01f;
                laser.Timer = 0.2f;
                if (laser.ComponentsCount == 0)
                    laser.AddComponent(GetComponent("decay"));
                pos += diff;
            }
        }

        public void CreateAsteroidSpawner()
        {
            var spawner = ReuseOrCreate(Tags.Spawner);
            if (spawner.ComponentsCount == 0) spawner.AddComponent(GetComponent("spawner"));
        }

        public Entity CreateUfo()
        {
            var ufo = ReuseOrCreate(Tags.Ufo, 15);
            if (ufo.ComponentsCount != 0) return ufo;
            ufo.AddComponent(GetComponent("ufo_ai"));
            ufo.Scale = 0.025f;
            ufo.SetCollider(GetCollider("ufo"));
            return ufo;
        }

        public void CreateLaserCounter()
        {
            for (var i = 0.0f; i < 3.0f; i++)
            {
                var laser = ReuseOrCreate(Tags.Ui, 10);
                laser.Scale = 0.025f;
                laser.Position = new Vector2(-1f + 0.025f + 0.05f * i, 1f - 0.075f);
                laser.AddComponent(GetComponent("laser_charge"));
                laser.Timer = i;
            }
        }

        public void CreateScoreUi()
        {
            var score = ReuseOrCreate(Tags.Ui, 12);
            score.Scale = 0.030f;
            score.Position = new Vector2(-1.0f + 0.1f, 1f - 0.025f);
            for (var i = 0.0f; i < 10.0f; i++)
            {
                var digit = ReuseOrCreate(Tags.Ui, 0);
                digit.Scale = 0.030f;
                digit.Position = new Vector2(-1.0f + 0.250f + 0.052f * i, 1f - 0.025f);
                digit.Timer = i;
                digit.AddComponent(GetComponent("score"));
            }

            var bigScore = ReuseOrCreate(Tags.Ui, 12);
            bigScore.Scale = 0.060f;
            bigScore.Position = new Vector2(-1.0f + 0.2f, -0.35f);
            bigScore.AddComponent(GetComponent("game_over"));
            for (var i = 0.0f; i < 10.0f; i++)
            {
                var bigDigit = ReuseOrCreate(Tags.Ui, 0);
                bigDigit.Scale = 0.060f;
                bigDigit.Position = new Vector2(-1.0f + 0.5f + 0.1f * i, -0.35f);
                bigDigit.Timer = i;
                bigDigit.AddComponent(GetComponent("score"));
                bigDigit.AddComponent(GetComponent("game_over"));
            }
        }

        #endregion
    }
}