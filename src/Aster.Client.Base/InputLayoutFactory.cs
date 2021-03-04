using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace Aster.Client.Base
{
    public sealed class InputLayoutFactory : IInputLayoutFactory
    {
        public InputLayout CreateInputLayout(VertexType vertexType)
        {
            var attributes = new List<VertexAttribute>();

            switch (vertexType)
            {
                case VertexType.Position:
                    attributes.Add(new VertexAttribute("i_position", 0, VertexAttribType.Float, 3, 0));
                    break;
                case VertexType.PositionColor:
                    attributes.Add(new VertexAttribute("i_position", 0, VertexAttribType.Float, 3, 0));
                    attributes.Add(new VertexAttribute("i_color", 1, VertexAttribType.Float, 4, 12));
                    break;
                case VertexType.PositionColorTexture:
                    attributes.Add(new VertexAttribute("i_position", 0, VertexAttribType.Float, 3, 0));
                    attributes.Add(new VertexAttribute("i_color", 1, VertexAttribType.Float, 4, 12));
                    attributes.Add(new VertexAttribute("i_uv", 2, VertexAttribType.Float, 2, 28));
                    break;
                case VertexType.PositionTexture:
                    attributes.Add(new VertexAttribute("i_position", 0, VertexAttribType.Float, 3, 0));
                    attributes.Add(new VertexAttribute("i_uv", 1, VertexAttribType.Float, 2, 12));
                    break;
                case VertexType.PositionTextureNormalTangent:
                    attributes.Add(new VertexAttribute("i_position", 0, VertexAttribType.Float, 3, 0));
                    attributes.Add(new VertexAttribute("i_uv", 1, VertexAttribType.Float, 2, 12));
                    attributes.Add(new VertexAttribute("i_normal", 2, VertexAttribType.Float, 3, 20));
                    attributes.Add(new VertexAttribute("i_tangent", 3, VertexAttribType.Float, 3, 32));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(vertexType), vertexType, null);
            }

            return new InputLayout(attributes);
        }
    }
}
