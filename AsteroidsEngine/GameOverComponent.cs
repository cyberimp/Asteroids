namespace AsteroidsEngine
{
    public class GameOverComponent : UpdateComponent
    {
        private readonly GuiVariables _variables;
        public GameOverComponent(GuiVariables variables)
        {
            _variables = variables;
        }
        public override void Update(Entity entity, float delta)
        {
            entity.Visible = _variables.GameOver;
        }
    }
}