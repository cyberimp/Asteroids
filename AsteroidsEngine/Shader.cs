using System;
using System.IO;
using System.Reflection;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace AsteroidsEngine
{
    public class Shader:IDisposable
    {
        private readonly int _handle;

        
        public Shader(string vertexPath, string fragmentPath)
        {
            var a = Assembly.GetExecutingAssembly();
            var myName = a.GetName().Name;
            
            string vertexShaderSource;

            using (var resource = a.GetManifestResourceStream(myName + "." + vertexPath))
            using (var reader = new StreamReader(resource ?? throw new Exception()))
            {
                vertexShaderSource = reader.ReadToEnd();
            }

            string fragmentShaderSource;

            using (var resource = a.GetManifestResourceStream(myName + "." + fragmentPath))
            using (var reader = new StreamReader(resource ?? throw new Exception()))
            {
                fragmentShaderSource = reader.ReadToEnd();
            }
            
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            
            
            GL.CompileShader(vertexShader);

            var infoLogVert = GL.GetShaderInfoLog(vertexShader);
            if (infoLogVert != string.Empty)
                Console.WriteLine(infoLogVert);

            GL.CompileShader(fragmentShader);

            var infoLogFrag = GL.GetShaderInfoLog(fragmentShader);

            if (infoLogFrag != string.Empty)
                Console.WriteLine(infoLogFrag);
            
            _handle = GL.CreateProgram();

            GL.AttachShader(_handle, vertexShader);
            GL.AttachShader(_handle, fragmentShader);

            GL.LinkProgram(_handle);
            
            
            GL.DetachShader(_handle, vertexShader);
            GL.DetachShader(_handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
        }
        
        public void Use()
        {
            GL.UseProgram(_handle);
        }

        public int GetAttribLocation(string name)
        {
            return GL.GetAttribLocation(_handle, name);
        }

        #region Disposing Methods

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            
            GL.DeleteProgram(_handle);

            _disposedValue = true;
        }

        ~Shader()
        {
            GL.DeleteProgram(_handle);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        
    }
}