using System;
using System.IO;
using OpenTK.Mathematics;
using Serilog;

namespace Aster.Client.World
{
    public sealed class ChunkLoader : IChunkLoader
    {
        private readonly ILogger _logger;

        public ChunkLoader(ILogger logger)
        {
            _logger = logger.ForContext<ChunkLoader>();
        }

        public Chunk LoadChunk(string filePath)
        {
            if (!File.Exists(filePath))
            {
                _logger.Error("Unable to load chunk from {@FilePath}", filePath);
                return null;
            }

            using var fileStream = File.OpenRead(filePath);
            using var reader = new BinaryReader(fileStream);

            var planetId = reader.ReadString();
            var positionX = reader.ReadInt32();
            var positionY = reader.ReadInt32();

            var tiles = new Tile[Chunk.ChunkSizeSquared];
            for (var i = 0; i < tiles.Length; i++)
            {
                tiles[i].Id = reader.ReadInt32();
            }

            return new Chunk(Guid.Parse(planetId), new Vector2i(positionX, positionY), tiles);
        }
    }
}
