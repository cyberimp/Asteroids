using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using Rectangle = System.Drawing.Rectangle;

namespace AsteroidsEngine
{
   public class Texture:IDisposable
    {
        private readonly int _handle;
        
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;
        private float[] _vertices;
        private uint[] _indices;

        private List<string> _names; 

        // Create texture from path.
        public Texture(string path)
        {
            _names = new List<string>();
            GenIndices(path);
                
            // Generate handle
            _handle = GL.GenTexture();

            var a = Assembly.GetExecutingAssembly();
            var myName = a.GetName().Name;
            // Bind the handle
            Use();

            // For this example, we're going to use .NET's built-in System.Drawing library to load textures.

            // Load the image
            using (var stream = a.GetManifestResourceStream(myName + "." + path + ".png"))
            using (var image = new Bitmap(stream)) {
                var data = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0,
                    PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0);
            }
            
            //Nearest for crisp pixelart
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            InitBuffers();
        }

        private void GenIndices(string path)
        {
            var indices = new List<uint>();
            uint firstIndex = 0;
            uint[] indicesStencil = { 0,1,3,1,2,3 };
            var vertices = new List<float>();
            var a = Assembly.GetExecutingAssembly();
            var myName = a.GetName().Name;
            using (var stream = a.GetManifestResourceStream(myName + "." + path + ".txt"))
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

                if (s == null) throw new FileLoadException("cannot read atlas, line:"+line);
                if (!s.StartsWith("size:"))
                    throw new FileLoadException("cannot read atlas, line:"+line);
                var nums = s.Substring(5).Split('x');
                if (nums.Length < 2 )
                    throw new FileLoadException("cannot read atlas, line:"+line);
                var sizeX = 1.0f/int.Parse(nums[0]);    
                var sizeY = 1.0f/int.Parse(nums[1]);
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
                        throw new FileLoadException("cannot read atlas, line:"+line);
                    }

                    split = size.Substring(1).Split(',');
                    var xcoord = int.Parse(split[0]);
                    var ycoord = int.Parse(split[1]);

                    //Clockwise generation  of quad coordinates
                    for (var f = MathHelper.PiOver4; f > -MathHelper.ThreePiOver2; 
                        f -= MathHelper.PiOver2)
                    {
                        vertices.Add(width*MathF.Sign(MathF.Cos(f)));
                        vertices.Add(height*MathF.Sign(MathF.Sin(f)));
                        vertices.Add(0.0f);
                        vertices.Add(xcoord*sizeX+width*sizeX*
                                     ((1.0f + MathF.Sign(MathF.Cos(f)))/2));
                        vertices.Add(ycoord*sizeY+height*sizeY*
                                     ((1.0f - MathF.Sign(MathF.Sin(f)))/2));

                    }

                    indices.AddRange(indicesStencil.Select(i => i + firstIndex));
                    
                    _names.Add(name);

                    firstIndex += 4;
                    
                    s = reader.ReadLine();
                    ++line;
                }

                _vertices = vertices.ToArray();
                _indices = indices.ToArray();
            }
        }

        private void InitBuffers()
        {
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

            // Because there's now 5 floats between the start of the first vertex and the start of the second,
            // we modify this from 3 * sizeof(float) to 5 * sizeof(float).
            // This will now pass the new vertex array to the buffer.
            var vertexLocation = ServiceLocator.GetShader().GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the first vertex coordinate comes after the first vertex
            // and change the amount of data to 2 because there's only 2 floats for vertex coordinates
            var texCoordLocation = ServiceLocator.GetShader().GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public void RenderQuad(string name)
        {
            RenderQuad(_names.FindIndex(s => s == name));
        }
        
        public void RenderQuad(int num)
        {
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, 6, 
                DrawElementsType.UnsignedInt, num*6*sizeof(float));
        }
        
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            
            GL.DeleteBuffer(_vertexArrayObject);
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            GL.DeleteTexture(_handle);

            _disposedValue = true;
        }

        ~Texture()
        {
            GL.DeleteBuffer(_vertexArrayObject);
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            GL.DeleteTexture(_handle);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int Length()
        {
            return _names.Count;
        }
    }
}