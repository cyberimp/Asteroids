namespace AsteroidsEngine
{
    public class ScoreDigitComponent: UpdateComponent
    {
        public override void Update(Entity entity, float delta)
        {
            var collection = ServiceLocator.GetEntities();
            var score = ServiceLocator.GetVariables().Score;
            for (var i = 9; i > entity.Timer; i--)
                score /= 10;
            var digit = score % 10;
            entity.SetRender(collection.GetRender(digit));

        }
    }
}