using System;

using Silk.NET.OpenGL;

namespace Cherry.Engine
{
    public class BufferObject<TDataType> : IDisposable where TDataType : unmanaged
    {
        private uint _id;
        private BufferTargetARB _bufferType;
        private GL _gl;

        public unsafe BufferObject(GL gl, Span<TDataType> data, BufferTargetARB bufferType)
        {
            _gl = gl;
            _bufferType = bufferType;
            _id = _gl.GenBuffer();
            Bind();
            fixed (void* d = data)
            {
                _gl.BufferData(_bufferType, (nuint)(data.Length * sizeof(TDataType)), d, BufferUsageARB.StaticDraw);
            }
        }
        public void Bind()
        {
            _gl.BindBuffer(_bufferType, _id);
        }

        public void Dispose()
        {
            _gl.DeleteBuffer(_id);
        }
    }
}