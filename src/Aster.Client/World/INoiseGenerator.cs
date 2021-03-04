using OpenTK.Mathematics;

namespace Aster.Client.World
{
    public interface INoiseGenerator
    {
        void GetNoiseData(Vector2i chunkPosition, out NoiseData noiseData);
    }
}
