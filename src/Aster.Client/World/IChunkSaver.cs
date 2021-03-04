namespace Aster.Client.World
{
    public interface IChunkSaver
    {
        void SaveChunk(Chunk chunk, string filePath);
    }
}
