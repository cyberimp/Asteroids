using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace AsteroidsEngine
{
    public class Engine : GameWindow
    {
        private const int DefaultXRes = 600;
        private const int DefaultYRes = 600;
        private const string AppName = "ASTEROIDS";
        private readonly Color4 _backColor;

        private EntityCollection _entities;

        private EngineSettings _settings;

        private Shader _shader;
        private Texture _texture;
        private bool _waitRestart;


        public Engine(int xRes = DefaultXRes, int yRes = DefaultYRes):
            base(xRes, yRes, GraphicsMode.Default, AppName, GameWindowFlags.FixedWindow)
        {
            _settings = new EngineSettings(xRes, yRes);
            _backColor = new Color4( 13, 8, 13,255);//Eroge Copper Black
        }



        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(_backColor);

            _shader = new Shader("shader.vert", "shader.frag");
            ServiceLocator.SetShader(_shader);
            _shader.Use();

            _texture = new Texture("atlas");
            ServiceLocator.SetTexture(_texture);
            _texture.Use();
            
            _entities = new EntityCollection();           
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha,BlendingFactor.OneMinusSrcAlpha);

            _entities.CreatePlayer();
            _entities.CreateAsteroidSpawner();
            _entities.CreateLaserCounter();
            _entities.CreateScoreUi();
            
            ServiceLocator.SetEngine(this);
            ServiceLocator.SetEntities(_entities);
            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            _shader.Dispose();
            _texture.Dispose();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
          
            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = AppName + " fps:" + 1/e.Time ;
            
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _texture.Use();
            _shader.Use();
            
            _entities.Render();
            
            
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _entities.Update((float) e.Time);
            _entities.Collide("bullet","asteroid");
            _entities.Collide("bullet","ufo");
            _entities.Collide("laser","asteroid");
            _entities.Collide("laser","ufo");
            _entities.Collide("ufo","player");
            _entities.Collide("asteroid","player");
            base.OnUpdateFrame(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            var controller = ServiceLocator.GetController();
            switch (e.Key)
            {
                case Key.Escape:
                    Exit();
                    break;
                case Key.Left:
                    controller.Rotation = -1;
                    break;
                case Key.Right:
                    controller.Rotation = 1;
                    break;
                case Key.Up:
                    controller.Thrust = true;
                    break;
                case Key.Z:
                    controller.Fire1 = true;
                    break;
                case Key.X:
                    controller.Fire2 = true;
                    break;
                case Key.F:
                    if (_waitRestart)
                    {
                        RestartGame();
                    }
                    break;
                        
            }
            base.OnKeyDown(e);
        }

        private void RestartGame()
        {
            _entities.CleanUp();
            _entities.CreatePlayer();
            _entities.FindByTag("spawner").Timer = 0.0f;
            ServiceLocator.GetVariables().LaserCharges = 2;
            ServiceLocator.GetVariables().Score = 0;
            ServiceLocator.GetVariables().GameOver = false;
            _waitRestart = false;
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            var controller = ServiceLocator.GetController();
            
            switch (e.Key)
            {
                case Key.Left:
                    controller.Rotation = 0;
                    break;
                case Key.Right:
                    controller.Rotation = 0;
                    break;
                case Key.Up:
                    controller.Thrust = false;
                    break;
                case Key.Z:
                    controller.Fire1 = false;
                    break;
                case Key.X:
                    controller.Fire2 = false;
                    break;
            }
            base.OnKeyDown(e);
        }

        public void GameOver()
        {
            _entities.CreateBanner();
            ServiceLocator.GetVariables().GameOver = true;
            _waitRestart = true;
        }
    }
}