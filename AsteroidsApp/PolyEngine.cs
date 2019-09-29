using System;
using AsteroidsEngine;
using OpenTK.Input;

namespace AsteroidsApp
{
    public class PolyEngine : Engine
    {
        private Model _model;
        private Texture _texture;
        public bool SpriteMode { get; private set; } = true;

        protected override void SetupResources()
        {
            base.SetupResources();

            _model = new Model(_shader,"poly");
            _model.GenIndices();
            _model.InitBuffers();

            _texture = new Texture(_shader,"atlas");
            _texture.GenIndices();
            _texture.InitBuffers();
            _texture.Use();
            
            var numRenders = _texture.Length();
            for (var i = 0; i < numRenders; i++)
            {
                var render = new PolyRenderComponent(i, _model, _texture, this);
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
                _texture.Use();
            }
            else
            {
                _model.Use();
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            _texture.Dispose();
            _model.Dispose();
            base.OnUnload(e);
        }
    }
}