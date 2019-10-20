namespace AsteroidsEngine
{
    public class UfoCollider : ICollider
    {
        private readonly GuiVariables _variables;

        public UfoCollider(GuiVariables variables)
        {
            _variables = variables;
        }

        public void OnCollide(Entity entity1, Entity entity2)
        {
            if (entity2.Tag != Tags.Bullet && entity2.Tag != Tags.Laser) return;
            entity1.Active = false;
            _variables.Score += 5;
        }
    }
}