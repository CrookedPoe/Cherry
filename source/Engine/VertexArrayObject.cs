using System;

using Silk.NET.OpenGL;

namespace Cherry.Engine
{
    public class VertexArrayObject<TVertexType, TIndexType> : IDisposable where TVertexType : unmanaged where TIndexType : unmanaged
    {
        private uint _id;
        private GL _gl;

        public VertexArrayObject(GL gl, BufferObject<TVertexType> vbo, BufferObject<TIndexType> ebo)
        {
            _gl = gl;
            _id = _gl.GenVertexArray();
            Bind();
            vbo.Bind();
            ebo.Bind();
        }
        public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, uint vertexSize, int offset)
        {
            _gl.VertexAttribPointer(index, count, type, false, vertexSize * (uint) sizeof(TVertexType), (void*) (offset * sizeof(TVertexType)));
            _gl.EnableVertexAttribArray(index);
        }

        public void Bind()
        {
            _gl.BindVertexArray(_id);
        }

        public void Dispose()
        {
            _gl.DeleteVertexArray(_id);
        }
    }
}