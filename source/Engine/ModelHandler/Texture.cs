using System;

using Silk.NET.OpenGL;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Cherry.Engine.ModelHandler
{
    public class Texture : IDisposable
    {
        private uint _id;
        private GL _gl;

        public unsafe Texture(GL gl, string path)
        {
            _gl = gl;
            _id = _gl.GenTexture();
            Bind();

            using (var img = Image.Load<Rgba32>(path))
            {
                gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)img.Width, (uint)img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
                img.ProcessPixelRows(p =>
                {
                    for (int y = 0; y < p.Height; y++)
                    {
                        fixed (void* data = p.GetRowSpan(y))
                        {
                            gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, y, (uint)p.Width, 1, PixelFormat.Rgba, PixelType.UnsignedByte, data);
                        }
                    }
                });
            }

            SetParameters();
        }

        public unsafe Texture(GL gl, Span<byte> data, uint width, uint height)
        {
            _gl = gl;
            _id = _gl.GenTexture();
            Bind();

            fixed (void* d = &data[0])
            {
                _gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d);
                SetParameters();
            }
        }
        
        private void SetParameters(int tileModeS = (int)GLEnum.Repeat, int tileModeT = (int)GLEnum.Repeat)
        {
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, tileModeS);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, tileModeT);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) GLEnum.LinearMipmapLinear);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) GLEnum.Linear);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);
            _gl.GenerateMipmap(TextureTarget.Texture2D);
        }
        public void Bind(TextureUnit slot = TextureUnit.Texture0)
        {
            _gl.ActiveTexture(slot);
            _gl.BindTexture(TextureTarget.Texture2D, _id);
        }

        public void Dispose()
        {
            _gl.DeleteTexture(_id);
        }
    }
}