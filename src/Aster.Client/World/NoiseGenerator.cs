using OpenTK.Mathematics;
using Serilog;

namespace Aster.Client.World
{
    public sealed class NoiseGenerator : INoiseGenerator
    {
        private readonly ILogger _logger;

        private readonly FastNoiseLite _noise;

        public NoiseGenerator(ILogger logger, ChunkCreatorOptions chunkCreatorOptions)
        {
            _logger = logger;
            _noise = new FastNoiseLite(chunkCreatorOptions.Seed);
            _noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            _noise.SetSeed(chunkCreatorOptions.Seed);
            _noise.SetFrequency(0.05f);

            _noise.SetFractalOctaves(5);
            _noise.SetFractalLacunarity(3.0f);
            _noise.SetFractalType(FastNoiseLite.FractalType.Ridged);
            _noise.SetFractalGain(0.5f);
            _noise.SetCellularJitter(0.1f);
            _noise.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.Euclidean);
        }

        public void GetNoiseData(Vector2i chunkPosition, out NoiseData noiseData)
        {
            _logger.Information("Get noise for chunk {@ChunkX},{@ChunkY}", chunkPosition.X, chunkPosition.Y);
            noiseData = new NoiseData(Chunk.ChunkSize, Chunk.ChunkSize);

            for (var y = 0; y < Chunk.ChunkSize; y++)
            {
                for (var x = 0; x < Chunk.ChunkSize; x++)
                {
                    var tx = chunkPosition.X * Chunk.ChunkSize + x; // / (float)Chunk.ChunkSize;
                    var ty = chunkPosition.Y * Chunk.ChunkSize + y; // / (float)Chunk.ChunkSize;

                    if (x == 0 && y == 0)
                    {
                        _logger.Information("{@ChunkX} {@ChunkY} - {@Tx} {@Ty}", chunkPosition.X, chunkPosition.Y, tx, ty);
                    }

                    var noiseValue = (float)_noise.GetNoise(tx, ty);
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
