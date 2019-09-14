using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<Entity> _newEntities;

        public Engine(int xRes = DefaultXRes, int yRes = DefaultYRes):
            base(xRes, yRes, GraphicsMode.Default, AppName, GameWindowFlags.FixedWindow)
        {
 
            _newEntities = new List<Entity>();
            _settings = new EngineSettings(xRes, yRes);
            _backColor = new Color4( 13, 8, 13,255);//Eroge Copper Black
        }

//        public Entity CreatePlayer()
//        {
//            var playerEntity = new Entity(Vector2.Zero) {Scale = 0.05f, Tag = "player"};
//            var renderComponent = new RenderComponent(14);
//            playerEntity.SetRender(renderComponent);
//            playerEntity.AddComponent(new PlayerComponent());
//            playerEntity.AddComponent(new HyperDriveComponent());
//            _entities.AddLast(playerEntity);
//            return playerEntity;
//        }
//
//        public Entity CreateAsteroid()
//        {
//            var asteroid = new Entity(Vector2.One*0.9f)
//            {
//                Scale = 0.25f, 
//                Velocity = new Vector2(0.1f, 0.1f),
//                Tag = "asteroid"
//            };
//            asteroid.AddComponent(new HyperDriveComponent());
//            asteroid.SetRender(new RenderComponent(16));
//            _newEntities.Add(asteroid);
//            return asteroid;
//        }
//
//        public Entity CreateBullet(Entity ship)
//        {
//            
//            if (_bullet == null)
//                _bullet = new RenderComponent(17);
//
//            var result = _entities.FirstOrDefault(
//                entity => entity.Tag == "bullet" && !entity.Active);
//            
//            if (result == null)
//            {
//                result = new Entity(ship.Position) {Tag = "bullet", Scale = 0.005f};
//                result.SetRender(_bullet);
//                result.AddComponent(new DecayComponent());
//                _newEntities.Add(result);
//            }
//
//            result.Active = true;
//            result.Timer = 0.5f;
//            result.Position = ship.Position;
//            result.Velocity = Vector2.Transform(Vector2.UnitY, 
//                Quaternion.FromAxisAngle(Vector3.UnitZ, 
//                    MathHelper.DegreesToRadians(ship.Rotation)));
//            
//            return result;
//        }


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
            _entities.CreateAsteroid();
            
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
                case Key.Space:
                    controller.Fire1 = true;
                    break;
                case Key.LControl:
                    controller.Fire2 = true;
                    break;
            }
            base.OnKeyDown(e);
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
                case Key.Space:
                    controller.Fire1 = false;
                    break;
                case Key.LControl:
                    controller.Fire2 = false;
                    break;
            }
            base.OnKeyDown(e);
        }

    }
}