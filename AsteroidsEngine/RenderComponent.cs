namespace AsteroidsEngine
{
    public class RenderComponent: EntityComponent
    {

        private readonly Texture _texture;
        private readonly int _quadNum;

        public RenderComponent(int quadNum)
        {
            _texture = ServiceLocator.GetTexture();
            _quadNum = quadNum;
        }
        
        public override void Render(Entity entity)
        {
            _texture.RenderQuad(_quadNum);
        }

        public override void Update(Entity entity, float delta)
        {
        }
    }
}