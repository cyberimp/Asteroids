using System;
using System.Collections.Generic;
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

        public void CreatePlayer()
        {
            _entities.AddLast(new Entity(Vector2.Zero));
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
            
            Matrix4 v =Matrix4.CreateScale(0.150f,0.200f,1.0f);
            _shader.SetMatrix4("transform",v);
   
            _texture.RenderQuad(0);

            
           // v = Matrix4.CreateRotationZ(45.0f);
           // GL.UniformMatrix4(transform,true,ref v);
            v =  Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(45f)) * Matrix4.CreateTranslation(1f,0.0f,0.0f)
                * Matrix4.CreateScale(0.150f,0.200f,1.0f);
            _shader.SetMatrix4("transform",v);
            
            _texture.RenderQuad(1);

            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            foreach (var entity in _entities)
            {
                entity.Update((float) e.Time);
            }
        }

        public void SetTranslationMatrix(Matrix4 trans)
        {
            _shader.SetMatrix4("transform", trans);
        }
    }
}