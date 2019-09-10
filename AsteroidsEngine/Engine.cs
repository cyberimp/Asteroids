using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace AsteroidsEngine
{
    public class Engine : GameWindow
    {
        private const int DefaultXRes = 800;
        private const int DefaultYRes = 600;
        private const string AppName = "ASTEROIDS";
        private readonly Color _backColor;

        private LinkedList<Entity> _entities;

        private EngineSettings _settings;

        private Shader _shader;
        private Texture _texture;

        public Engine(int xRes = DefaultXRes, int yRes = DefaultYRes):
            base(xRes, yRes, GraphicsMode.Default, AppName, GameWindowFlags.FixedWindow)
        {
            _entities = new LinkedList<Entity>();
            _settings = new EngineSettings(xRes, yRes);
            _backColor = Color.FromArgb(255, 13, 8, 13);//Eroge Copper Black
        }

        public Entity CreatePlayer()
        {
            var playerEntity = new Entity(Vector2.Zero);
            var renderComponent = new RenderComponent();
            playerEntity.AddComponent(renderComponent);
            playerEntity.AddComponent(new PlayerComponent());
            _entities.AddLast(playerEntity);
            return playerEntity;
        }


        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(_backColor);


            _shader = new Shader("shader.vert", "shader.frag");
            ServiceLocator.SetShader(_shader);
            _shader.Use();


            _texture = new Texture("asteroids.png");
            ServiceLocator.SetTexture(_texture);
            _texture.Use();
            
            
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha,BlendingFactor.OneMinusSrcAlpha);

            CreatePlayer();
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
            GL.Clear(ClearBufferMask.ColorBufferBit);



            _texture.Use();
            _shader.Use();
            
            foreach (var entity in _entities)
            {
                entity.Render();
            }

            

            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            foreach (var entity in _entities)
            {
                entity.Update((float) e.Time);
            }
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
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            var controller = ServiceLocator.GetController();
            
            switch (e.Key)
            {
                case Key.Escape:
                    Exit();
                    break;
                case Key.Left:
                    controller.Rotation = 0;
                    break;
                case Key.Right:
                    controller.Rotation = 0;
                    break;
            }
            base.OnKeyDown(e);
        }

        public void SetTranslationMatrix(Matrix4 trans)
        {
            _shader.SetMatrix4("transform", trans);
        }
    }
}