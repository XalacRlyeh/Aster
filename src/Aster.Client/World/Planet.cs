using System;
using OpenTK.Mathematics;

namespace Aster.Client.World
{
    public class Planet
    {
        private readonly IChunkProvider _chunkProvider;

        internal Planet(IChunkProvider chunkProvider, Guid id)
            : this(id)
        {
            _chunkProvider = chunkProvider;
        }

        public Planet(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        public int Seed { get; set; }

        public Chunk GetChunk(Vector2i position)
        {
            return _chunkProvider.GetChunk(Id, position);
        }
    }
}
