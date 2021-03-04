using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Aster.Client.Base
{
    public sealed class MeshFactory : IMeshFactory
    {
        public Mesh CreateUnitCubeMesh()
        {
            var cubeColor0 = new Vector4(112 / 256f, 53 / 256f, 63 / 256f, 1.0f);
            var cubeColor1 = new Vector4(132 / 256f, 53 / 256f, 63 / 256f, 1.0f);
            var cubeColor2 = new Vector4(152 / 256f, 53 / 256f, 63 / 256f, 1.0f);
            var cubeColor3 = new Vector4(172 / 256f, 53 / 256f, 63 / 256f, 1.0f);
            var cubeColor4 = new Vector4(192 / 256f, 53 / 256f, 63 / 256f, 1.0f);
            var cubeColor5 = new Vector4(212 / 256f, 53 / 256f, 63 / 256f, 1.0f);
            var cubeVertices = new List<VertexPositionColor>
            {
                new (new Vector3(-1.0f, -1.0f, -1.0f), cubeColor0), // Front
                new (new Vector3(-1.0f, 1.0f, -1.0f), cubeColor0),
                new (new Vector3(1.0f, 1.0f, -1.0f), cubeColor0),
                new (new Vector3(-1.0f, -1.0f, -1.0f), cubeColor0),
                new (new Vector3(1.0f, 1.0f, -1.0f), cubeColor0),
                new (new Vector3(1.0f, -1.0f, -1.0f), cubeColor0),

                new (new Vector3(-1.0f, -1.0f, 1.0f), cubeColor1), // BACK
                new (new Vector3(1.0f, 1.0f, 1.0f), cubeColor1),
                new (new Vector3(-1.0f, 1.0f, 1.0f), cubeColor1),
                new (new Vector3(-1.0f, -1.0f, 1.0f), cubeColor1),
                new (new Vector3(1.0f, -1.0f, 1.0f), cubeColor1),
                new (new Vector3(1.0f, 1.0f, 1.0f), cubeColor1),

                new (new Vector3(-1.0f, 1.0f, -1.0f), cubeColor2), // Top
                new (new Vector3(-1.0f, 1.0f, 1.0f), cubeColor2),
                new (new Vector3(1.0f, 1.0f, 1.0f), cubeColor2),
                new (new Vector3(-1.0f, 1.0f, -1.0f), cubeColor2),
                new (new Vector3(1.0f, 1.0f, 1.0f), cubeColor2),
                new (new Vector3(1.0f, 1.0f, -1.0f), cubeColor2),

                new (new Vector3(-1.0f, -1.0f, -1.0f), cubeColor3), // Bottom
                new (new Vector3(1.0f, -1.0f, 1.0f), cubeColor3),
                new (new Vector3(-1.0f, -1.0f, 1.0f), cubeColor3),
                new (new Vector3(-1.0f, -1.0f, -1.0f), cubeColor3),
                new (new Vector3(1.0f, -1.0f, -1.0f), cubeColor3),
                new (new Vector3(1.0f, -1.0f, 1.0f), cubeColor3),

                new (new Vector3(-1.0f, -1.0f, -1.0f), cubeColor4), // Left
                new (new Vector3(-1.0f, -1.0f, 1.0f), cubeColor4),
                new (new Vector3(-1.0f, 1.0f, 1.0f), cubeColor4),
                new (new Vector3(-1.0f, -1.0f, -1.0f), cubeColor4),
                new (new Vector3(-1.0f, 1.0f, 1.0f), cubeColor4),
                new (new Vector3(-1.0f, 1.0f, -1.0f), cubeColor4),

                new (new Vector3(1.0f, -1.0f, -1.0f), cubeColor5), // Right
                new (new Vector3(1.0f, 1.0f, 1.0f), cubeColor5),
                new (new Vector3(1.0f, -1.0f, 1.0f), cubeColor5),
                new (new Vector3(1.0f, -1.0f, -1.0f), cubeColor5),
                new (new Vector3(1.0f, 1.0f, -1.0f), cubeColor5),
                new (new Vector3(1.0f, 1.0f, 1.0f), cubeColor5)
            };

            var cubeVertexBuffer = new Buffer<VertexPositionColor>(cubeVertices.ToArray(), BufferTarget.ArrayBuffer);

            return new Mesh(cubeVertexBuffer, Enumerable.Empty<MeshPart>().ToArray(), Enumerable.Empty<Material>().ToArray());
        }
    }
}
