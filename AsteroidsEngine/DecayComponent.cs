namespace AsteroidsEngine
{
    public class DecayComponent : UpdateComponent
    {
        public override bool Update(Entity entity, float delta)
        {
            entity.Timer -= delta;
            return (entity.Timer > 0.0f);
        }
    }
}