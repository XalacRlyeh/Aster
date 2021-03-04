using System;
using OpenTK.Mathematics;
using Serilog;

namespace Aster.Client.World
{
    public sealed class ChunkCreator : IChunkCreator
    {
        private readonly ILogger _logger;
        private readonly INoiseGenerator _noiseGenerator;
        private readonly Random _random;

        private Tile[] _deepWaterTiles;
        private Tile[] _shallowWaterTiles;
        private Tile[] _coastTiles;
        private Tile[] _sandTiles;
        private Tile[] _grassTiles;

        public ChunkCreator(
            ILogger logger,
            INoiseGenerator noiseGenerator,
            ChunkCreatorOptions chunkCreatorOptions)
        {
            _logger = logger;
            _noiseGenerator = noiseGenerator;
            _random = new Random(chunkCreatorOptions.Seed);

            _deepWaterTiles = new[]
            {
                new Tile { Id = 0 },
                new Tile { Id = 1 },
                new Tile { Id = 2 },
                new Tile { Id = 3 }
            };
            _shallowWaterTiles = new[]
            {
                new Tile { Id = 64 },
                new Tile { Id = 65 },
                new Tile { Id = 66 },
                new Tile { Id = 67 }
            };
            _coastTiles = new[]
            {
                new Tile { Id = 128 },
                new Tile { Id = 129 },
                new Tile { Id = 130 },
                new Tile { Id = 131 }
            };
            _sandTiles = new[]
            {
                new Tile { Id = 192 },
                new Tile { Id = 193 },
                new Tile { Id = 194 },
                new Tile { Id = 195 }
            };
            _grassTiles = new[]
            {
                new Tile { Id = 256 },
                new Tile { Id = 257 },
                new Tile { Id = 258 },
                new Tile { Id = 259 }
            };
        }

        public Chunk CreateChunk(Guid planetId, Vector2i position)
        {
            var tiles = new Tile[Chunk.ChunkSizeSquared];

            _noiseGenerator.GetNoiseData(position, out var noiseData);

            for (var y = 0; y < Chunk.ChunkSize; y++)
            {
                for (var x = 0; x < Chunk.ChunkSize; x++)
                {
                    var normalizedNoiseValue = noiseData.Data[y * Chunk.ChunkSize + x];
                    normalizedNoiseValue = (normalizedNoiseValue - noiseData.Min) / (noiseData.Max - noiseData.Min);

                    tiles[y * Chunk.ChunkSize + x] = NoiseToTileId(normalizedNoiseValue);
                }
            }

            return new Chunk(planetId, position, tiles);
        }

        private Tile NoiseToTileId(float noiseValue)
        {
            if (noiseValue < 0.1f)
            {
                return RandomDeepWaterTile();
            }

            if (noiseValue >= 0.1f && noiseValue < 0.15f)
            {
                return RandomShallowWaterTile();
            }

            if (noiseValue >= 0.15f && noiseValue < 0.18f)
            {
                return RandomCoastWaterTile();
            }

            if (noiseValue >= 0.18f && noiseValue < 0.25f)
            {
                return RandomSandTile();
            }

            if (noiseValue >= 0.25f && noiseValue < 0.65f)
            {
                return RandomGrassTile();
            }

            return new Tile { Id = 4 };
        }

        private Tile RandomDeepWaterTile()
        {
            return _deepWaterTiles[_random.Next(_deepWaterTiles.Length)];
        }

        private Tile RandomShallowWaterTile()
        {
            return _shallowWaterTiles[_random.Next(_shallowWaterTiles.Length)];
        }

        private Tile RandomCoastWaterTile()
        {
            return _coastTiles[_random.Next(_coastTiles.Length)];
        }

        private Tile RandomSandTile()
        {
            return _sandTiles[_random.Next(_sandTiles.Length)];
        }

        private Tile RandomGrassTile()
        {
            return _grassTiles[_random.Next(_grassTiles.Length)];
        }
    }
}
