using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;

namespace Aster.Client.Base
{
    public sealed class ConstantBuffer : IDisposable
    {
        private readonly int _bufferSize;
        private readonly int _nativeBuffer;

        public static implicit operator int(ConstantBuffer constantBuffer)
        {
            return constantBuffer._nativeBuffer;
        }

        public static ConstantBuffer Create<T>(T data) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var buffer = new ConstantBuffer(size);
            buffer.Initialize(data);
            return buffer;
        }

        public void Bind(int index)
        {
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, index, _nativeBuffer);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_nativeBuffer);
        }

        private ConstantBuffer(int bufferSize)
        {
            _bufferSize = bufferSize;

            GL.CreateBuffers(1, out _nativeBuffer);
        }

        private void Initialize<T>(T data) where T : struct
        {
            GL.NamedBufferStorage(_nativeBuffer, _bufferSize, ref data, BufferStorageFlags.DynamicStorageBit);
        }

        public void UpdateBuffer<T>(T data) where T : struct
        {
            GL.NamedBufferSubData(_nativeBuffer, IntPtr.Zero, _bufferSize, ref data);
        }
    }
}
