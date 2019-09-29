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

        protected Shader _shader;
        protected EntityCollection Entities;
        public GuiVariables Variables {get;}
        public Controller CurController{get;}

        private bool _waitRestart;
        protected Engine(int xRes = DefaultXRes, int yRes = DefaultYRes) :
            base(xRes, yRes, GraphicsMode.Default, AppName, GameWindowFlags.FixedWindow)
        {
            _backColor = new Color4(13, 8, 13, 255); //Eroge Copper Black
            Variables = new GuiVariables();
            CurController = new Controller();
        }


        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(_backColor);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            SetupResources();

            Entities.CreatePlayer();
            Entities.CreateAsteroidSpawner();
            Entities.CreateLaserCounter();
            Entities.CreateScoreUi();
            Entities.CreateBanner();

            base.OnLoad(e);
        }

        protected virtual void SetupResources()
        {
            _shader = new Shader("shader1.vert", "shader1.frag");
            _shader.Use();

            Entities = new EntityCollection(_shader, this);
            Entities.FillColliders();
            Entities.FillComponents();
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
            Title = AppName + " fps:" + 1 / e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();

            Entities.Render();


            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Entities.Update((float) e.Time);
            Entities.Collide(Tags.Bullet, Tags.Asteroid);
            Entities.Collide(Tags.Bullet, Tags.Ufo);
            Entities.Collide(Tags.Laser, Tags.Asteroid);
            Entities.Collide(Tags.Laser, Tags.Ufo);
            Entities.Collide(Tags.Ufo, Tags.Player);
            Entities.Collide(Tags.Asteroid, Tags.Player);
            base.OnUpdateFrame(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Exit();
                    break;
                case Key.Left:
                    CurController.Rotation = -1;
                    break;
                case Key.Right:
                    CurController.Rotation = 1;
                    break;
                case Key.Up:
                    CurController.Thrust = true;
                    break;
                case Key.Z:
                    CurController.Fire1 = true;
                    break;
                case Key.X:
                    CurController.Fire2 = true;
                    break;
                case Key.F:
                    if (_waitRestart) RestartGame();
                    break;
            }

            base.OnKeyDown(e);
        }

        
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    CurController.Rotation = 0;
                    break;
                case Key.Right:
                    CurController.Rotation = 0;
                    break;
                case Key.Up:
                    CurController.Thrust = false;
                    break;
                case Key.Z:
                    CurController.Fire1 = false;
                    break;
                case Key.X:
                    CurController.Fire2 = false;
                    break;
            }

            base.OnKeyDown(e);
        }

private void RestartGame()
        {
            Entities.CleanUp();
            Entities.CreatePlayer();
            var spawner = Entities.FindByTag(Tags.Spawner);
            spawner.Timer = 0.0f;
            spawner.Active = true;

            Variables.LaserCharges = MaxLaserCharges;
            Variables.Score = 0;
            Variables.GameOver = false;
            _waitRestart = false;
        }

        public void GameOver()
        {
            Entities.FindByTag(Tags.Spawner).Active = false;
            Variables.GameOver = true;
            _waitRestart = true;
        }
    }
}