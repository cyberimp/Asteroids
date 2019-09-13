namespace AsteroidsEngine
{
    public class DecayComponent:EntityComponent
    {
        public override void Render(Entity entity)
        {
        }

        public override void Update(Entity entity, float delta)
        {
            entity.Timer -= delta;
            if (entity.Timer <= 0.0f)
                entity.Active = false;
        }
    }
}