namespace AsteroidsEngine
{
    public class BulletCollider : ICollider
    {
        public void OnCollide(Entity entity1, Entity entity2)
        {
            entity1.Active = false;
        }
    }
}