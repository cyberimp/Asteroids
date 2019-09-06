namespace AsteroidsEngine
{
    public abstract class EntityComponent
    {
        private bool Active { get; set; }
        
        public abstract void Render(Entity entity, float delta);
        public abstract void Update(Entity entity, float delta);

    }
}