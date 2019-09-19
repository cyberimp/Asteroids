namespace AsteroidsEngine
{
    public class ShipCollider : ICollider
    {
        public void OnCollide(Entity entity1, Entity entity2)
        {
            if (entity2.Tag != "asteroid" && entity2.Tag != "ufo") return;
            entity1.Active = false;
            ServiceLocator.GetEngine().GameOver();
        }
    }
}