using System;
using OpenTK.Mathematics;

namespace Aster.Client.World
{
    public interface IChunkCreator
    {
        Chunk CreateChunk(Guid planetId, Vector2i position);
    }
}
