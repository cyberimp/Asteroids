using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace AsteroidsEngine
{
    public class Engine : GameWindow
    {
        private const int DefaultXRes = 800;
        private const int DefaultYRes = 600;
        private const string AppName = "ASTEROIDS";

        private EngineSettings _settings;

        public Engine(int xRes = DefaultXRes, int yRes = DefaultYRes):
            base(xRes, yRes, GraphicsMode.Default, AppName, GameWindowFlags.FixedWindow)
        {
            _settings = new EngineSettings(xRes, yRes);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
            GL.Begin(PrimitiveType.Lines);
            GL.PointSize(10);
                GL.Color3(Color.White);
                GL.Vertex2(0,0);
                GL.Vertex2(1,1);
                GL.End();
                
            SwapBuffers();
        }
    }
}