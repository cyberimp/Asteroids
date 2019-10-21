namespace AsteroidsEngine
{
    public class GameOverComponent : UpdateComponent
    {
        private readonly GuiVariables _variables;

        public GameOverComponent(GuiVariables variables)
        {
            _variables = variables;
        }

        public override bool Update(Entity entity, float delta)
        {
            entity.Visible = _variables.GameOver;
            return true;
        }
    }
}