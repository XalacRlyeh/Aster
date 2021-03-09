using System.Collections.Generic;
using System.Linq;
using Aster.Client.Base;
using Aster.Client.World;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Aster.Client
{
    public class ChunkMeshFactory
    {
        private readonly TextureAtlas _textureAtlas;

        public ChunkMeshFactory(TextureAtlas textureAtlas)
        {
            _textureAtlas = textureAtlas;
        }

        public Mesh CreateChunkMesh(IReadOnlyCollection<Chunk> visibleChunks)
        {
            const int tileSize = Tile.TileSize;
            var vertices = new List<VertexPositionColorTexture>();
            foreach (var visibleChunk in visibleChunks)
            {
                var chunkPosition = visibleChunk.Position;

                for (var i = 0; i < visibleChunk.Tiles.Length; ++i)
                {
                    var tileId = visibleChunk.Tiles[i].Id;
                    var uvs = _textureAtlas.GetTileUvs(tileId);

                    var tileX = i % Chunk.ChunkSize;
                    var tileY = i / Chunk.ChunkSize;

                    var tX = chunkPosition.X * Chunk.ChunkSize * Tile.TileSize + (tileX * Tile.TileSize);
                    var tY = chunkPosition.Y * Chunk.ChunkSize * Tile.TileSize + (tileY * Tile.TileSize);

                    var color = tileX == Chunk.ChunkSize - 1 || tileY == Chunk.ChunkSize - 1
                        ? new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
                        : new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

                    vertices.Add(new VertexPositionColorTexture(new Vector3(tX, tY, 0.0f), color, uvs[0]));
                    vertices.Add(new VertexPositionColorTexture(new Vector3(tX + tileSize, tY, 0.0f), color, uvs[1]));
                    vertices.Add(new VertexPositionColorTexture(new Vector3(tX, tY + tileSize, 0.0f), color, uvs[2]));

                    vertices.Add(new VertexPositionColorTexture(new Vector3(tX + tileSize, tY, 0.0f), color, uvs[1]));
                    vertices.Add(new VertexPositionColorTexture(new Vector3(tX + tileSize, tY + tileSize, 0.0f), color, uvs[3]));
                    vertices.Add(new VertexPositionColorTexture(new Vector3(tX, tY + tileSize, 0.0f), color, uvs[2]));
                }
            }

            var vertexBuffer = new Buffer<VertexPositionColorTexture>(vertices.ToArray(), BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            return new Mesh(vertexBuffer, Enumerable.Empty<MeshPart>().ToArray(), Enumerable.Empty<Material>().ToArray());
        }
    }
}
