using AsteroidsEngine;
using OpenTK.Graphics.OpenGL;

namespace AsteroidsApp
{
    public class Model:Texture
    {
        private float[] _vertices =
        {
            0f, 1f, 0f, 0f, 0f,
            1f, -1f, 0f, 0f, 0f,
            0f, -0.5f, 0f, 0f, 0f,
            -1f, -1f, 0f, 0f, 0f
        };
        private int[] _indices =
        {
            0, 1,
            1, 2,
            2, 3,
            3, 0
        };
        
        public Model(string path) : base(path)
        {
        }

        public override void GenIndices()
        {
        }

        public override void InitBuffers()
        {
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            var vertexLocation = ServiceLocator.GetShader().GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = ServiceLocator.GetShader().GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        }

        public void RenderModel(int num)
        {
            GL.DrawElements(PrimitiveType.Lines,8,DrawElementsType.UnsignedInt,0);
            
        }
    }
}