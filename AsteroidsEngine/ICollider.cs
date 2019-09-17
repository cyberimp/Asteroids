namespace AsteroidsEngine
{
    public interface ICollider
    {
        void OnCollide(Entity entity1, Entity entity2);
    }
}