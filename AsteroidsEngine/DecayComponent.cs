namespace AsteroidsEngine
{
    public class DecayComponent:UpdateComponent
    {
        public override void Update(Entity entity, float delta)
        {
            entity.Timer -= delta;
            if (entity.Timer <= 0.0f)
                entity.Active = false;
        }
    }
}