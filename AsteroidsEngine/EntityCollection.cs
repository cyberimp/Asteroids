using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace AsteroidsEngine
{
    public class EntityCollection
    {
        private readonly string[] _renderTagOrder = 
            {"","bullet","asteroid","ufo","laser","player", "banner", "score"};
        private readonly LinkedList<Entity> _collection;
        private List<RenderComponent> _renders;
        private readonly List<Entity> _newCollection;
        private readonly Dictionary<string, UpdateComponent> _components;
        private readonly Dictionary<string, ICollider> _colliders;

        public EntityCollection()
        {
            _collection = new LinkedList<Entity>();
            _newCollection = new List<Entity>();
            _components = new Dictionary<string, UpdateComponent>();
            _colliders = new Dictionary<string, ICollider>();
            FillComponents();
            FillColliders();
            FillRenders();
        }

        private void FillColliders()
        {
            AddCollider("bullet", new BulletCollider());
            AddCollider("asteroid", new AsteroidCollider());
            AddCollider("ship", new ShipCollider());
        }

        private void FillRenders()
        {
            _renders = new List<RenderComponent>();
            var numRenders = ServiceLocator.GetTexture().Length();
            for (var i = 0; i < numRenders; i++)
            {
                var render = new RenderComponent(i);
                _renders.Add(render);
            }
        }

        private void FillComponents()
        {
            AddComponent("hyper",new HyperDriveComponent());
            AddComponent("decay", new DecayComponent());
            AddComponent("player", new PlayerComponent());
            AddComponent("a_spawner", new AsteroidSpawnerComponent());
            AddComponent("ufo_ai", new UfoAiComponent());
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

        private Entity ReuseOrCreate(string tag, int render)
        {
            var result = ReuseOrCreate(tag);
            result.SetRender(_renders[render]);

            return result;
        }

        private Entity ReuseOrCreate(string tag)
        {
            var result = _collection.FirstOrDefault(entity => entity.Tag == tag && !entity.Active);
            if (result != null)
                result.Active = true;
            else
            {
                result = new Entity(Vector2.Zero) {Tag = tag};
                _newCollection.Add(result);
            }

            return result;
        }

        #region Factory Methods

        public Entity CreatePlayer()
        {
            var player = ReuseOrCreate("player", 14);
            player.Position = Vector2.Zero;
            player.Velocity = Vector2.Zero;
            player.Rotation = 0.0f;
            if (player.ComponentsCount != 0) return player;
            player.Scale = 0.025f;
            player.AddComponent(GetComponent("player"));
            player.AddComponent(GetComponent("hyper"));
            player.SetCollider(GetCollider("ship"));

            return player;
        }

        public Entity CreateAsteroid()
        {
            var asteroid = ReuseOrCreate("asteroid", 16);
            asteroid.Scale = 0.1f;
            asteroid.Position = -Vector2.UnitX * 0.8f;
            asteroid.Velocity = Vector2.One*0.2f;
            if (asteroid.ComponentsCount != 0) return asteroid;
            
            asteroid.AddComponent(GetComponent("hyper"));
            asteroid.SetCollider(GetCollider("asteroid"));

            return asteroid;
        }

        public Entity CreateBullet(Entity origin)
        {
            var bullet = ReuseOrCreate("bullet", 17);
            bullet.Timer = 0.5f;
            bullet.Scale = 0.005f;
            bullet.Position = origin.Position;
            bullet.Velocity = Vector2.Transform(Vector2.UnitY, 
                        Quaternion.FromAxisAngle(Vector3.UnitZ, 
                            MathHelper.DegreesToRadians(origin.Rotation)));
            if (bullet.ComponentsCount > 0) return bullet;
            
            
            bullet.AddComponent(GetComponent("decay"));
            bullet.AddComponent(GetComponent("hyper"));
            bullet.SetCollider(GetCollider("bullet"));
            
            return bullet;
        }

        public void CreateBanner()
        {
            var banner = ReuseOrCreate("banner", 13);
            banner.Active = true;
            banner.Scale = 0.25f;
        }

        public void CreateLaser(Entity origin)
        {
            var rot = origin.Rotation; 
            var diff = Vector2.Transform(Vector2.UnitY*0.02f,
                Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(rot)));

            var pos = origin.Position;
            while (Vector2.Clamp(pos, -Vector2.One, Vector2.One) == pos) 
                
            {
                var laser = ReuseOrCreate("laser",11);
                laser.Position = pos;
                laser.Rotation = rot;
                laser.Scale = 0.01f;
                laser.Timer = 0.2f;
                if (laser.ComponentsCount == 0)
                    laser.AddComponent(GetComponent("decay"));
                pos += diff;
            }
        }

        public Entity CreateAsteroidSpawner()
        {
            var spawner = ReuseOrCreate("spawner");
            if (spawner.ComponentsCount == 0)
            {
                spawner.AddComponent(GetComponent("a_spawner"));
            }
            return spawner;
        }

        public Entity CreateUfo()
        {
            var ufo = ReuseOrCreate("ufo",15);
            if (ufo.ComponentsCount != 0) return ufo;
            ufo.AddComponent(GetComponent("ufo_ai"));
            ufo.Scale = 0.025f;
            ufo.SetCollider(GetCollider("bullet"));
            return ufo;
        }

        #endregion

        public void Update(float delta)
        {
            if (_newCollection.Count > 0)
            {
                foreach (var entity in _newCollection)
                {
                    _collection.AddLast(entity);
                }
                _newCollection.Clear();
            }

            foreach (var entity in _collection)
            {
                entity.Update(delta);
            }
            
        }

        public void Render()
        {
            foreach (var tag in _renderTagOrder)
                foreach (var entity in _collection.Where(entity => entity.Tag == tag))
                    entity.Render();
        }

        public void Collide(string tag1, string tag2)
        {
            foreach (var entity1 in _collection.Where(entity => entity.Tag == tag1 && entity.Active))
            {
                var cachePos = entity1.Position;
                var cacheSize = entity1.Scale * 2;
                foreach (var entity2 in _collection.Where(entity => entity.Tag == tag2 && entity.Active))
                    if (Vector2.DistanceSquared(cachePos, entity2.Position) <
                        (cacheSize + entity2.Scale*2) * (cacheSize + entity2.Scale*2))
                    {
                        entity1.Collide(entity2);
                        entity2.Collide(entity1);
                    }
                        
            }
        }


        public void CleanUp()
        {
            foreach (var trash in _collection.Where(entity => entity.Tag == "asteroid" ||
                                                                 entity.Tag == "laser" ||
                                                                 entity.Tag == "bullet" ||
                                                                 entity.Tag == "banner" ||
                                                                 entity.Tag == "ufo"))
            {
                trash.Active = false;
            }
        }

        public Entity FindByTag(string tag)
        {
            return _collection.FirstOrDefault(entity => entity.Tag == tag);
        }
    }
}