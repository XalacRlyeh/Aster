using System;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace Aster.Client.Base
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct VertexPositionTextureNormalTangent : IEquatable<VertexPositionTextureNormalTangent>
    {
        public readonly Vector3 Position;

        public readonly Vector2 Uv;

        public readonly Vector3 Normal;

        public readonly Vector3 Tangent;

        public VertexPositionTextureNormalTangent(Vector3 position, Vector2 uv, Vector3 normal, Vector3 tangent)
        {
            Position = position;
            Uv = uv;
            Normal = normal;
            Tangent = tangent;
        }

        public bool Equals(VertexPositionTextureNormalTangent other)
        {
            return Uv.Equals(other.Uv) &&
                   Normal.Equals(other.Normal) &&
                   Tangent.Equals(other.Tangent) &&
                   Position.Equals(other.Position);
        }

        public override bool Equals(object obj)
        {
            return obj is VertexPositionTextureNormalTangent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Uv, Normal, Tangent);
        }
    }
}
