namespace AsteroidsEngine
{
    public class ScoreDigitComponent : UpdateComponent
    {
        private readonly GuiVariables _variables;
        private readonly EntityCollection _parent;
        public ScoreDigitComponent(GuiVariables variables, EntityCollection parent)
        {
            _variables = variables;
            _parent = parent;
        }
        public override void Update(Entity entity, float delta)
        {
            var score = _variables.Score;
            for (var i = 9; i > entity.Timer; i--)
                score /= 10;
            var digit = score % 10;
            entity.SetRender(_parent.GetRender(digit));
        }
    }
}