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

        private LinkedList<Entity> _entities;

        private EngineSettings _settings;
        
        Shader shader;

        public Engine(int xRes = DefaultXRes, int yRes = DefaultYRes):
            base(xRes, yRes, GraphicsMode.Default, AppName, GameWindowFlags.FixedWindow)
        {
            _entities = new LinkedList<Entity>();
            _settings = new EngineSettings(xRes, yRes);
        }

        public void CreatePlayer()
        {
            _entities.AddLast(new Entity(Vector2.Zero));
        }

        private int VertexBufferObject;
        private int VertexArrayObject;
        private readonly float[] vertices = {0.5f, -0.5f, 0.5f, 0.5f, -0.5f, -0.5f, -0.5f, 0.5f
        };

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.Aquamarine);
            
            shader = new Shader("shader.vert", "shader.frag");
            
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer,VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, 
                BufferUsageHint.StaticDraw);
            
            VertexArrayObject = GL.GenVertexArray();
            
            // ..:: Initialization code (done once (unless your object frequently changes)) :: ..
// 1. bind Vertex Array Object
            GL.BindVertexArray(VertexArrayObject);
// 2. copy our vertices array in a buffer for OpenGL to use
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
// 3. then set our vertex attributes pointers
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float)*2, 0);
            GL.EnableVertexAttribArray(0);
            
            GL.Enable(EnableCap.LineSmooth);

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            shader.Dispose();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
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

//            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
//            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
//
//            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
//            GL.EnableVertexAttribArray(0);

            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            
            
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
    }
}