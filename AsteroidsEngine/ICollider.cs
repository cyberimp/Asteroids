namespace AsteroidsEngine
{
    public interface ICollider
    {
        bool OnCollide(Entity entity1, Entity entity2);
    }
}