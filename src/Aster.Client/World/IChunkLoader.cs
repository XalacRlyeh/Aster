namespace Aster.Client.World
{
    public interface IChunkLoader
    {
        Chunk LoadChunk(string filePath);
    }
}
