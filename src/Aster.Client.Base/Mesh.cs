using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenTK.Graphics.OpenGL4;

namespace Aster.Client.Base
{
    public class Mesh : IDisposable
    {
        public Buffer VertexBuffer { get; }

        public ReadOnlyCollection<Material> Materials { get; }

        public ReadOnlyCollection<MeshPart> MeshParts { get; }

        public Mesh(Buffer vertexBuffer, IList<MeshPart> meshParts, IList<Material> materials)
        {
            Materials = new ReadOnlyCollection<Material>(materials);
            MeshParts = new ReadOnlyCollection<MeshPart>(meshParts);
            VertexBuffer = vertexBuffer;
        }

        public void Dispose()
        {
            VertexBuffer.Dispose();
        }

        public void Draw()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, VertexBuffer.ElementCount);
        }
    }
}
