namespace AsteroidsEngine
{
    public class UfoCollider : ICollider
    {
        private readonly GuiVariables _variables;

        public UfoCollider(GuiVariables variables)
        {
            _variables = variables;
        }

        public bool OnCollide(Entity entity1, Entity entity2)
        {
            if (entity2.Tag != Tags.Bullet && entity2.Tag != Tags.Laser)
            {
                return true;
            }

            _variables.Score += 5;
            return false;
        }
    }
}