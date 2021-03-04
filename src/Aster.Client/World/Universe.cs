using System;
using OpenTK.Mathematics;
using Serilog;

namespace Aster.Client.World
{
    public class Universe
    {
        private readonly ILogger _logger;
        private readonly IPlanetProvider _planetProvider;

        public Universe(
            ILogger logger,
            IPlanetProvider planetProvider)
        {
            _logger = logger;
            _planetProvider = planetProvider;
        }

        public int Seed { get; set; }

        public Chunk GetChunk(Guid planetId, Vector2i position)
        {
            var planet = _planetProvider.GetPlanet(planetId);
            planet.Seed = Seed;
            return planet.GetChunk(position);
        }

        public Tile GetTile(Guid planetId, Vector2i position)
        {
            var planet = _planetProvider.GetPlanet(planetId);
            var planetChunk = planet.GetChunk(position);

            return planetChunk.GetTile(position);
        }

        public void Tick()
        {
        }
    }
}
