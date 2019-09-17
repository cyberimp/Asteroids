using AsteroidsEngine;
using OpenTK.Input;

namespace AsteroidsApp
{
    public class PolyEngine: Engine
    {
        public bool SpriteMode { get; private set; } = true;

        protected override void SetupResources()
        {
            base.SetupResources();
            ServiceLocator.SetEngine(this);
            var numRenders = ServiceLocator.GetTexture().Length();
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