using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpenGlProj
{
    internal class Shader
    {
        public int Handle;

        private bool disposedValue = false;

        public Shader()
        {
            var error = GL.GetError();

            var VertexShader = GL.CreateShader(ShaderType.VertexShader);
            error = GL.GetError();
            GL.ShaderSource(VertexShader, GLSLDefines.shaderVert);
            error = GL.GetError();

            var FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            error = GL.GetError();
            GL.ShaderSource(FragmentShader, GLSLDefines.shaderFrag);
            error = GL.GetError();

            int success;

            GL.CompileShader(VertexShader);
            error = GL.GetError();

            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out success);
            error = GL.GetError();
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(FragmentShader);
            error = GL.GetError();

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
            error = GL.GetError();
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }

            Handle = GL.CreateProgram();
            error = GL.GetError();

            GL.AttachShader(Handle, VertexShader);
            error = GL.GetError();
            GL.AttachShader(Handle, FragmentShader);
            error = GL.GetError();

            GL.LinkProgram(Handle);
            error = GL.GetError();

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
            error = GL.GetError();
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

        }

        public int GetAttribLocation(string attribName)
        {
            var error = GL.GetError();
            var res = GL.GetAttribLocation(Handle, attribName);
            error = GL.GetError();
            return res;
        }

        public void SetInt(string name, int value)
        {
            var error = GL.GetError();
            int location = GL.GetUniformLocation(Handle, name);
            error = GL.GetError();
            GL.Uniform1(location, value);
            error = GL.GetError();
        }


        public void Use()
        {
            var error = GL.GetError();
            GL.UseProgram(this.Handle);
            error = GL.GetError();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
