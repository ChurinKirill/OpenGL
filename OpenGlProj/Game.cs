using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpenGlProj
{
    public class Game : GameWindow
    {

        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;

        Shader shader;
        //int vertexColorLocation;
        Texture texture1;
        Texture texture2;


        float[] vertices =
        {
            //Position          Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        Stopwatch timer = new Stopwatch();
        long prevTicks;

        public Game(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = new Vector2i(width, height), Title = title }) { }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);            

            GL.ActiveTexture(TextureUnit.Texture0); // activate the texture unit first before binding texture
            GL.BindTexture(TextureTarget.Texture2D, 0);

            this.VertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.VertexBufferObject);
            //GL.BufferData(
            //    BufferTarget.ArrayBuffer,
            //    this.vertices.Length * sizeof(float),
            //    this.vertices,
            //    BufferUsageHint.StaticDraw
            //    );

            shader = new Shader();
            shader.Use();

            var error = GL.GetError();

            //this.vertexColorLocation = GL.GetUniformLocation(this.shader.Handle, "ourColor");

            this.VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            error = GL.GetError();
            int vertexLocation = shader.GetAttribLocation("aPosition");
            error = GL.GetError();
            GL.VertexAttribPointer(
                vertexLocation,
                3,
                VertexAttribPointerType.Float,
                false,
                5 * sizeof(float),
                0
                );
            GL.EnableVertexAttribArray(vertexLocation);

            error = GL.GetError();
            int texCoordLocation = shader.GetAttribLocation("aTexCoord");
            error = GL.GetError();
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(
                texCoordLocation,
                2,
                VertexAttribPointerType.Float,
                false,
                5 * sizeof(float),
                3 * sizeof(float)
                );
            GL.EnableVertexAttribArray(texCoordLocation);
            error = GL.GetError();
            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(uint),
                indices,
                BufferUsageHint.StaticDraw);

            error = GL.GetError();
            shader.SetInt("texture1", 0);
            error = GL.GetError();
            shader.SetInt("texture2", 1);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            float[] borderColor = { 1.0f, 1.0f, 0.0f, 1.0f };
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            error = GL.GetError();
            //this.shader.Use();

            this.texture1 = new Texture("D:\\Kirill\\Projects\\OpenGlProj\\Textures\\container.jpg");
            this.texture2 = new Texture("D:\\Kirill\\Projects\\OpenGlProj\\Textures\\wall.jpg");


            this.timer.Start();
            this.prevTicks = Stopwatch.GetTimestamp();

        }

        protected override void OnUnload()
        {
            base.OnUnload();
            this.shader.Dispose();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            

            GL.Clear(ClearBufferMask.ColorBufferBit);

            // calculating color
            //double timeValue = this.timer.Elapsed.TotalSeconds;
            //float greenValue = (float)Math.Sin(timeValue) / 2.0f + 0.5f;
            //GL.Uniform4(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);

            long curTicks = Stopwatch.GetTimestamp();

            float deltaT = (curTicks - this.prevTicks) / (float)Stopwatch.Frequency;

            var rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(-90.0f * deltaT));


            this.prevTicks = curTicks;

            for (int i = 0; i < vertices.Length; i += 5)
            {
                var newVector = Vector3.Transform(
                    new Vector3(this.vertices[i], this.vertices[i + 1], this.vertices[i + 2]),
                    rotation
                    );

                this.vertices[i] = newVector.X;
                this.vertices[i + 1] = newVector.Y;
                this.vertices[i + 2] = newVector.Z;
            }
            //GL.ActiveTexture(TextureUnit.Texture0);
            var error = GL.GetError();
            //texture.Use();
            //error = GL.GetError();
            //GL.Uniform1(shader.GetAttribLocation("texture1"), 0);
            //error = GL.GetError();
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                this.vertices.Length * sizeof(float),
                this.vertices,
                BufferUsageHint.StaticDraw
                );
            error = GL.GetError();
            GL.BindVertexArray(VertexArrayObject);

            this.texture1.Use(TextureUnit.Texture0);
            error = GL.GetError();
            this.texture2.Use(TextureUnit.Texture1);
            error = GL.GetError();
            error = GL.GetError();
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);


            SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }


    }
}
