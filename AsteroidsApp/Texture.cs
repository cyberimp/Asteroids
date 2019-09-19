using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using AsteroidsEngine;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace AsteroidsApp
{
    public class Texture : IDisposable
    {
        private readonly int _handle;

        private readonly List<string> _names;

        private protected readonly string Path;

        private bool _disposedValue;
        protected int ElementBufferObject;
        private protected uint[] Indices;
        protected int VertexArrayObject;

        protected int VertexBufferObject;
        private protected float[] Vertices;

        // Create texture from path.
        public Texture(string path)
        {
            _names = new List<string>();
            Path = path;
            _handle = GL.GenTexture();

            var a = Assembly.GetExecutingAssembly();
            var myName = a.GetName().Name;
            Use();

            using (var stream = a.GetManifestResourceStream(myName + "." + Path + ".png"))
            using (var image = new Bitmap(stream))
            {
                var data = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0);
            }

            //Nearest for crisp pixelart
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void GenIndices()
        {
            var indices = new List<uint>();
            uint firstIndex = 0;
            uint[] indicesStencil = {0, 1, 3, 1, 2, 3};
            var vertices = new List<float>();
            var a = Assembly.GetExecutingAssembly();
            var myName = a.GetName().Name;
            using (var stream = a.GetManifestResourceStream(myName + "." + Path + ".txt"))
            using (var reader = new StreamReader(stream ??
                                                 throw new FileNotFoundException("atlas not found")))
            {
                var line = 0;
                var s = reader.ReadLine();
                ++line;
                if (s == null) throw new FileLoadException("cannot read atlas");
                while (s != null && s[0] == '#')
                {
                    s = reader.ReadLine();
                    ++line;
                }

                if (s == null) throw new FileLoadException("cannot read atlas, line:" + line);
                if (!s.StartsWith("size:"))
                    throw new FileLoadException("cannot read atlas, line:" + line);
                var nums = s.Substring(5).Split('x');
                if (nums.Length < 2)
                    throw new FileLoadException("cannot read atlas, line:" + line);
                var sizeX = 1.0f / int.Parse(nums[0]);
                var sizeY = 1.0f / int.Parse(nums[1]);
                s = reader.ReadLine();
                ++line;
                while (!string.IsNullOrEmpty(s))
                {
                    var split = s.Split(":");
                    var name = split[0];
                    var size = split[1].Trim();

                    var sizeType = size[0];
                    var width = 1;
                    var height = 1;

                    switch (sizeType)
                    {
                        case 's':
                            break;
                        case 'l':
                            width = 4;
                            break;
                        case 'd':
                            width = 2;
                            height = 2;
                            break;
                        default:
                            throw new FileLoadException("cannot read atlas, line:" + line);
                    }

                    split = size.Substring(1).Split(',');
                    var xcoord = int.Parse(split[0]);
                    var ycoord = int.Parse(split[1]);

                    //Clockwise generation  of quad coordinates
                    for (var f = MathHelper.PiOver4;
                        f > -MathHelper.ThreePiOver2;
                        f -= MathHelper.PiOver2)
                    {
                        vertices.Add(width * MathF.Sign(MathF.Cos(f)));
                        vertices.Add(height * MathF.Sign(MathF.Sin(f)));
                        vertices.Add(0.0f);
                        vertices.Add(xcoord * sizeX + width * sizeX *
                                     ((1.0f + MathF.Sign(MathF.Cos(f))) / 2));
                        vertices.Add(ycoord * sizeY + height * sizeY *
                                     ((1.0f - MathF.Sign(MathF.Sin(f))) / 2));
                    }

                    indices.AddRange(indicesStencil.Select(i => i + firstIndex));

                    _names.Add(name);

                    firstIndex += 4;

                    s = reader.ReadLine();
                    ++line;
                }

                Vertices = vertices.ToArray();
                Indices = indices.ToArray();
            }
        }

        public void InitBuffers()
        {
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices,
                BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices,
                BufferUsageHint.StaticDraw);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            var vertexLocation = ServiceLocator.GetShader().GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = ServiceLocator.GetShader().GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float),
                3 * sizeof(float));

            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public void RenderQuad(string name)
        {
            RenderQuad(_names.FindIndex(s => s == name));
        }

        public void RenderQuad(int num)
        {
            GL.DrawElements(PrimitiveType.Triangles, 6,
                DrawElementsType.UnsignedInt, num * 6 * sizeof(float));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            GL.DeleteBuffer(VertexArrayObject);
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteTexture(_handle);

            _disposedValue = true;
        }

        ~Texture()
        {
            GL.DeleteBuffer(VertexArrayObject);
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteTexture(_handle);
        }

        public int Length()
        {
            return _names.Count;
        }
    }
}