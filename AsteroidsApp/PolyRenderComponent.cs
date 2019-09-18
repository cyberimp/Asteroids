using AsteroidsEngine;

namespace AsteroidsApp
{
    public class PolyRenderComponent : RenderComponent
    {
        private readonly Model _model;
        private readonly Texture _texture;

        public PolyRenderComponent(int quadNum) : base(quadNum)
        {
            _texture = ResourceLocator.GetTexture();
            _model = ResourceLocator.GetModel();
        }


        public override void Render()
        {
            if (((PolyEngine) ServiceLocator.GetEngine()).SpriteMode)
                RenderSprite();
            else
                RenderModel();
        }

        private void RenderSprite()
        {
            _texture.RenderQuad(QuadNum);
        }

        private void RenderModel()
        {
            _model.RenderModel(QuadNum);
        }
    }
}