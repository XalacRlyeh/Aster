using System.Collections.Generic;
using System.Linq;

namespace Aster.Client.Base
{
    public static class InputLayoutMapper
    {
        private static readonly IDictionary<VertexType, string[]> _inputLayouts;

        static InputLayoutMapper()
        {
            _inputLayouts = new Dictionary<VertexType, string[]>
            {
                {
                    VertexType.Unknown, new string[] { }
                },
                {
                    VertexType.Position, new[]
                    {
                        "i_position"
                    }
                },
                {
                    VertexType.PositionColor, new[]
                    {
                        "i_position",
                        "i_color"
                    }
                },
                {
                    VertexType.PositionColorTexture, new[]
                    {
                        "i_position",
                        "i_color",
                        "i_uv"
                    }
                },
                {
                    VertexType.PositionTexture, new[]
                    {
                        "i_position",
                        "i_uv"
                    }
                },
                {
                    VertexType.PositionTextureNormalTangent, new[]
                    {
                        "i_position",
                        "i_uv",
                        "i_normal",
                        "i_tangent"
                    }
                }
            };
        }

        public static VertexType Match(string[] attributeNames)
        {
            var sortedAttributeNames = attributeNames
                .OrderBy(attributeName => attributeName)
                .ToArray();

            foreach (var semanticMap in _inputLayouts)
            {
                var semanticNames = semanticMap.Value.OrderBy(attributeName => attributeName);
                if (semanticNames.SequenceEqual(sortedAttributeNames))
                {
                    return semanticMap.Key;
                }
            }

            return VertexType.Unknown;
        }
    }
}
