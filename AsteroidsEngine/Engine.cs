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
        public const int MaxLaserCharges = 3;
        private readonly Color4 _backColor;

        protected EntityCollection Entities;

        private Shader _shader;
//        private Texture _texture;
        private bool _waitRestart;


        protected Engine(int xRes = DefaultXRes, int yRes = DefaultYRes):
            base(xRes, yRes, GraphicsMode.Default, AppName, GameWindowFlags.FixedWindow)
        {
            _backColor = new Color4( 13, 8, 13,255);//Eroge Copper Black
        }



        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(_backColor);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha,BlendingFactor.OneMinusSrcAlpha);
            ServiceLocator.SetEngine(this);
            SetupResources();

            ServiceLocator.SetEntities(Entities);

            Entities.CreatePlayer();
            Entities.CreateAsteroidSpawner();
            Entities.CreateLaserCounter();
            Entities.CreateScoreUi();
            Entities.CreateBanner();
            
            base.OnLoad(e);
        }

        protected virtual void SetupResources()
        {
            _shader = new Shader("shader.vert", "shader.frag");
            ServiceLocator.SetShader(_shader);
            _shader.Use();

            Entities = new EntityCollection();
        }

        protected override void OnUnload(EventArgs e)
        {
            _shader.Dispose();
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

            _shader.Use();
            
            Entities.Render();
            
            
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Entities.Update((float) e.Time);
            Entities.Collide("bullet","asteroid");
            Entities.Collide("bullet","ufo");
            Entities.Collide("laser","asteroid");
            Entities.Collide("laser","ufo");
            Entities.Collide("ufo","player");
            Entities.Collide("asteroid","player");
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
            Entities.CleanUp();
            Entities.CreatePlayer();
            Entities.FindByTag("spawner").Timer = 0.0f;
            ServiceLocator.GetVariables().LaserCharges = MaxLaserCharges;
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
            ServiceLocator.GetVariables().GameOver = true;
            _waitRestart = true;
        }
    }
}