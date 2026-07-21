using System;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace OpenGlProj
{
    internal class Texture
    {
        public int Handle;

        public Texture(string path)
        {
            var error = GL.GetError();
            Handle = GL.GenTexture();
            error = GL.GetError();

            // stb_image loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            // This will correct that, making the texture display properly.
            StbImage.stbi_set_flip_vertically_on_load(1);
            error = GL.GetError();
            //this.Use();
            // Load the image.
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
            error = GL.GetError();

            GL.TexImage2D(
                TextureTarget.Texture2D, 
                0,
                PixelInternalFormat.Rgba, 
                image.Width,
                image.Height,
                0, 
                PixelFormat.Rgba, 
                PixelType.UnsignedByte,
                image.Data
                );
            error = GL.GetError();
            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            var error = GL.GetError();
            GL.ActiveTexture(unit);
            error = GL.GetError();
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            error = GL.GetError();
        }
    }
}
