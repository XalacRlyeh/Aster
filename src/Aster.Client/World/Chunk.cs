using System;
using OpenTK.Mathematics;

namespace Aster.Client.World
{
    public class Chunk
    {
        public const int ChunkSize = 64;
        public const int ChunkSizeSquared = ChunkSize * ChunkSize;

        public Chunk(Guid planetId, Vector2i position, Tile[] tiles)
        {
            PlanetId = planetId;
            Position = position;
            Tiles = tiles;
        }

        public DateTime LastLoadTime { get; set; }

        public Guid PlanetId { get; }

        public Vector2i Position { get; }

        public Tile[] Tiles { get; }

        public Tile GetTile(Vector2i position)
        {
            var localPosition = new Vector2i(position.X & ChunkSize, position.Y & ChunkSize);
            var tileIndex = localPosition.Y * ChunkSize + localPosition.X;

            return Tiles[tileIndex];
        }
    }
}
