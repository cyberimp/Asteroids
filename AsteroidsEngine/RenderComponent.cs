namespace AsteroidsEngine
{
    public class RenderComponent: EntityComponent
    {

        private Texture _texture;

        public RenderComponent()
        {
            _texture = ServiceLocator.GetTexture();
        }
        
        public override void Render(Entity entity)
        {
            _texture.RenderQuad(0);
        }

        public override void Update(Entity entity, float delta)
        {
        }
    }
}