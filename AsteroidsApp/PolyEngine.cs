using AsteroidsEngine;
using OpenTK.Input;

namespace AsteroidsApp
{
    public class PolyEngine: Engine
    {
        private Texture _texture;
        private Model _model;
        public bool SpriteMode { get; private set; } = true;

        protected override void SetupResources()
        {
            base.SetupResources();
            ServiceLocator.SetEngine(this);
            
            _model = new Model("model");
            _model.InitBuffers();
            _model.Use();

            _texture = new Texture("atlas");
            _texture.GenIndices();
            _texture.InitBuffers();
            _texture.Use();

            ResourceLocator.SetTexture(_texture);
            ResourceLocator.SetModel(_model);
            
            var numRenders = _texture.Length();
            for (var i = 0; i < numRenders; i++)
            {
                var render = new PolyRenderComponent(i);
                Entities.AddRender(render);
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                SwitchMode();
                return;
            }

            base.OnKeyDown(e);
        }

        private void SwitchMode()
        {
            SpriteMode = !SpriteMode;
            if (SpriteMode)
            {
                _texture.InitBuffers();
                _texture.Use();
            }
            else
            {
                _model.InitBuffers();
                _model.Use();
            }
        }
    }
}