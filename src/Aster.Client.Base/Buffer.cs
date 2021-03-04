using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;

namespace Aster.Client.Base
{
    public abstract class Buffer : IDisposable
    {
        private readonly int _id;

        public int ElementCount { get; protected set; }

        public int Stride { get; protected set; }

        protected Buffer()
        {
            GL.CreateBuffers(1, out _id);
        }

        public int Id
        {
            get { return _id; }
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_id);
        }
    }

    public class Buffer<T> : Buffer
        where T : struct
    {
        public Buffer(
            T[] items,
            BufferStorageFlags bufferStorageFlags = BufferStorageFlags.DynamicStorageBit)
            : base()
        {
            Stride = Marshal.SizeOf<T>();
            ElementCount = items.Length;
            GL.NamedBufferStorage(Id, Stride * ElementCount, items, bufferStorageFlags);
        }

        public Buffer(
            T[] items,
            BufferTarget bufferTarget,
            BufferUsageHint bufferUsageHint = BufferUsageHint.DynamicDraw)
            : base()
        {
            Stride = Marshal.SizeOf<T>();
            ElementCount = items.Length;

            GL.BindBuffer(bufferTarget, Id);
            GL.BufferData(bufferTarget, Stride * ElementCount, items, bufferUsageHint);
        }

        public void Bind()
        {
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, Id);
        }
    }
}
