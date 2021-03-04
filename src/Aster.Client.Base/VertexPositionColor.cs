using System;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace Aster.Client.Base
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct VertexPositionColor : IEquatable<VertexPositionColor>
    {
        public readonly Vector3 Position;

        public readonly Vector4 Color;

        public VertexPositionColor(Vector3 position, Vector4 color)
        {
            Position = position;
            Color = color;
        }

        public bool Equals(VertexPositionColor other)
        {
            return Position.Equals(other.Position) && Color.Equals(other.Color);
        }

        public override bool Equals(object obj)
        {
            return obj is VertexPositionColor other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Color);
        }
    }
}
