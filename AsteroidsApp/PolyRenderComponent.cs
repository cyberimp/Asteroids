using AsteroidsEngine;

namespace AsteroidsApp
{
    public class PolyRenderComponent : RenderComponent
    {
        private readonly Model _model;
        private readonly Texture _texture;
        private readonly PolyEngine _engine;

        public PolyRenderComponent(int quadNum, Model model, Texture texture, PolyEngine engine) : base(quadNum)
        {
            _texture = texture;
            _model = model;
            _engine = engine;
        }


        public override void Render()
        {
            if (_engine.SpriteMode)
            {
                RenderSprite();
            }
            else
            {
                RenderModel();
            }
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