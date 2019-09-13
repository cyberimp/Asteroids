using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AsteroidsEngine
{
    public class Shader:IDisposable
    {
        public readonly int Handle;

        private readonly Dictionary<string, int> _uniformLocations;

        
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
            
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            GL.LinkProgram(Handle);
            
            
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
            
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
            
            // Next, allocate the dictionary to hold the locations.
            _uniformLocations = new Dictionary<string, int>();

            // Loop over all the uniforms,
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                var key = GL.GetActiveUniform(Handle, i, out _, out _);

                // get the location,
                var location = GL.GetUniformLocation(Handle, key);
                
                // and then add it to the dictionary.
                _uniformLocations.Add(key, location);
            }
        }
        
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string name)
        {
            return GL.GetAttribLocation(Handle, name);
        }
        
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }

        #region Disposing Methods

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            
            GL.DeleteProgram(Handle);

            _disposedValue = true;
        }

        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        
    }
}