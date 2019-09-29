namespace AsteroidsEngine
{
    public class UfoCollider : ICollider
    {
        public void OnCollide(Entity entity1, Entity entity2)
        {
            if (entity2.Tag != Tags.Bullet && entity2.Tag != Tags.Laser) return;
            entity1.Active = false;
            ServiceLocator.GetVariables().Score += 5;
        }
    }
}