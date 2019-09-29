namespace AsteroidsEngine
{
    public class ShipCollider : ICollider
    {
        private readonly Engine _engine;
        public ShipCollider(Engine engine)
        {
            _engine = engine;
        }
        public void OnCollide(Entity entity1, Entity entity2)
        {
            if (entity2.Tag != Tags.Asteroid && entity2.Tag != Tags.Ufo) return;
            entity1.Active = false;
            _engine.GameOver();
        }
    }
}