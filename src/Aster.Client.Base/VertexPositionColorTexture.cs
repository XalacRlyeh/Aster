using System;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace Aster.Client.Base
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct VertexPositionColorTexture : IEquatable<VertexPositionColorTexture>
    {
        public readonly Vector3 Position;

        public readonly Vector4 Color;

        public readonly Vector2 Uv;

        public VertexPositionColorTexture(Vector3 position, Vector4 color, Vector2 uv)
        {
            Position = position;
            Color = color;
            Uv = uv;
        }

        public bool Equals(VertexPositionColorTexture other)
        {
            return Position.Equals(other.Position) && Color.Equals(other.Color) && Uv.Equals(other.Uv);
        }

        public override bool Equals(object obj)
        {
            return obj is VertexPositionColorTexture other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Color, Uv);
        }
    }
}
