using OpenTK.Mathematics;
using Serilog;

namespace Aster.Client.World
{
    public sealed class NoiseGenerator : INoiseGenerator
    {
        private readonly ILogger _logger;

        private readonly Noise _noise;

        public NoiseGenerator(ILogger logger, ChunkCreatorOptions chunkCreatorOptions)
        {
            _logger = logger;
            _noise = new Noise(chunkCreatorOptions.Seed);
        }

        public void GetNoiseData(Vector2i chunkPosition, out NoiseData noiseData)
        {
            _logger.Debug("Get noise for chunk {@ChunkX},{@ChunkY}", chunkPosition.X, chunkPosition.Y);
            noiseData = new NoiseData(Chunk.ChunkSize, Chunk.ChunkSize);

            for (var y = 0; y < Chunk.ChunkSize; y++)
            {
                for (var x = 0; x < Chunk.ChunkSize; x++)
                {
                    var tx = chunkPosition.X * Chunk.ChunkSize + x;
                    var ty = chunkPosition.Y * Chunk.ChunkSize + y;

                    var noiseValue = (float)_noise.GetValue(tx, ty);
                    if (noiseValue > noiseData.Max)
                    {
                        noiseData.Max = noiseValue;
                    }

                    if (noiseValue < noiseData.Min)
                    {
                        noiseData.Min = noiseValue;
                    }

                    noiseData.Data[y * Chunk.ChunkSize + x] = noiseValue;
                }
            }
        }
    }
}
