using System;
using OpenTK.Mathematics;

namespace Aster.Client.World
{
    public interface IChunkProvider
    {
        Chunk GetChunk(Guid planetId, Vector2i position);
    }
}
