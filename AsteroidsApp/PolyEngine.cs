using AsteroidsEngine;
using OpenTK.Input;

namespace AsteroidsApp
{
    public class PolyEngine: Engine
    {
        private Texture _texture;
        public bool SpriteMode { get; private set; } = true;

        protected override void SetupResources()
        {
            base.SetupResources();
            ServiceLocator.SetEngine(this);
            _texture = new Texture("atlas");
            _texture.Use();
            
            ResourceLocator.SetTexture(_texture);
            
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
                SpriteMode = !SpriteMode;
                return;
            }

            base.OnKeyDown(e);
        }
    }
}