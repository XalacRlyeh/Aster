using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Mathematics;
using Serilog;

namespace Aster.Client.World
{
    public sealed class ChunkProvider : IChunkProvider
    {
        private readonly ILogger _logger;
        private readonly ChunkProviderOptions _chunkProviderOptions;
        private readonly IChunkLoader _chunkLoader;
        private readonly IChunkSaver _chunkSaver;
        private readonly IChunkCreator _chunkCreator;
        private readonly IDictionary<Vector2i, Chunk> _chunks;

        public ChunkProvider(
            ILogger logger,
            ChunkProviderOptions chunkProviderOptions,
            IChunkLoader chunkLoader,
            IChunkSaver chunkSaver,
            IChunkCreator chunkCreator)
        {
            _logger = logger.ForContext<ChunkProvider>();
            _chunkProviderOptions = chunkProviderOptions;
            _chunkLoader = chunkLoader;
            _chunkSaver = chunkSaver;
            _chunkCreator = chunkCreator;
            _chunks = new Dictionary<Vector2i, Chunk>(8192);
        }

        public Chunk GetChunk(Guid planetId, Vector2i position)
        {
            if (_chunks.TryGetValue(position, out var chunk))
            {
                _logger.Debug("Retrieving chunk for planet {@Id} at {@X} {@Y} from cache", planetId, position.X, position.Y);
                chunk.LastLoadTime = DateTime.UtcNow;
                return chunk;
            }

            var chunkFilePath = Path.Combine(_chunkProviderOptions.StoragePath, planetId.ToString("N"), $"{position.Y}-{position.X}.chunk");
            if (TryLoadChunk(position, chunkFilePath, out chunk))
            {
                return chunk;
            }

            if (TryCreateChunk(planetId, position, chunkFilePath, out chunk))
            {
                return chunk;
            }

            _logger.Error("Unable to retrieve from cache/load from disk/generate chunk for planet {@Id} at position {@X} {@Y}", planetId, position.X, position.Y);
            return null;
        }

        private bool TryLoadChunk(Vector2i position, string chunkFilePath, out Chunk chunk)
        {
            chunk = _chunkLoader.LoadChunk(chunkFilePath);
            if (chunk != null)
            {
                _logger.Debug("Retrieved chunk for planet {@Id} at {@X} {@Y} from loader", chunk.PlanetId, position.X, position.Y);
                chunk.LastLoadTime = DateTime.Now;
                _chunks.Add(position, chunk);
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryCreateChunk(Guid planetId, Vector2i position, string chunkFilePath, out Chunk chunk)
        {
            chunk = _chunkCreator.CreateChunk(planetId, position);
            if (chunk != null)
            {
                _logger.Debug("Generated chunk for planet {@Id} at {@X} {@Y}", planetId, position.X, position.Y);
                _chunkSaver.SaveChunk(chunk, chunkFilePath);
                _chunks.Add(position, chunk);

                return true;
            }

            return false;
        }
    }
}
