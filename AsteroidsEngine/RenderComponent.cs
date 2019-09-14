namespace AsteroidsEngine
{
    public class RenderComponent
    {

        private readonly Texture _texture;
        private readonly int _quadNum;

        public RenderComponent(int quadNum)
        {
            _texture = ServiceLocator.GetTexture();
            _quadNum = quadNum;
        }
        
        public virtual void Render(Entity entity)
        {
            _texture.RenderQuad(_quadNum);
        }

    }
}