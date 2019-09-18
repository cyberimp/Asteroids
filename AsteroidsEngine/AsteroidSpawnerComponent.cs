using System;
using OpenTK;

namespace AsteroidsEngine
{
    public class AsteroidSpawnerComponent : UpdateComponent
    {
        private readonly Random _rnd = new Random();

        public override void Update(Entity entity, float delta)
        {
            entity.Timer -= delta;
            if (entity.Timer > 0.0f) return;
            entity.Timer = 6f;
            var collection = ServiceLocator.GetEntities();
            var shipPos = collection.FindByTag("player").Position;
            var ufo = collection.FindByTag("ufo");
            var spawnUfo = (ufo == null || !ufo.Active) && _rnd.Next(10) > 8;

            Vector2 newPos;
            do
            {
                newPos = new Vector2((float) (_rnd.NextDouble() * 1.8f - 0.9f),
                    (float) (_rnd.NextDouble() * 1.8f - 0.9f));
            } while (Vector2.DistanceSquared(newPos, shipPos) < 0.25f);

            var enemy = spawnUfo
                ? ServiceLocator.GetEntities().CreateUfo()
                : ServiceLocator.GetEntities().CreateAsteroid();
            enemy.Active = false;
            enemy.Position = newPos;
            enemy.Active = true;
            if (spawnUfo) return;


            enemy.Rotation = (float) (_rnd.NextDouble() * 90f);
            enemy.Velocity = Vector2.Transform(enemy.Velocity,
                Quaternion.FromAxisAngle(Vector3.UnitZ,
                    (float) (_rnd.NextDouble() * MathHelper.TwoPi)));
        }
    }
}