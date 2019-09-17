using AsteroidsEngine;

namespace AsteroidsApp
{
    public class PolyRenderComponent:RenderComponent
    {
        private readonly Texture _texture;
        public PolyRenderComponent(int quadNum) : base(quadNum)
        {
            _texture = ServiceLocator.GetTexture();
        }

        private void RenderSprite()
        {
            _texture.RenderQuad(QuadNum);
        }

        public override void Render()
        {
            if (((PolyEngine)ServiceLocator.GetEngine()).SpriteMode)
                RenderSprite();
        }
    }
}