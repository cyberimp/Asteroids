namespace AsteroidsEngine
{
    public class GameOverComponent:UpdateComponent
    {
        public override void Update(Entity entity, float delta)
        {
            entity.Visible = ServiceLocator.GetVariables().GameOver;
        }
    }
}