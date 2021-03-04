using System;
using System.Collections.Generic;
using Serilog;

namespace Aster.Client.World
{
    public sealed class PlanetProvider : IPlanetProvider
    {
        private readonly ILogger _logger;
        private readonly IChunkProvider _chunkProvider;

        private readonly IDictionary<Guid, Planet> _planets;

        public PlanetProvider(ILogger logger, IChunkProvider chunkProvider)
        {
            _logger = logger;
            _chunkProvider = chunkProvider;
            _planets = new Dictionary<Guid, Planet>(16);
        }

        public Planet GetPlanet(Guid planetId)
        {
            if (_planets.TryGetValue(planetId, out var planet))
            {
                return planet;
            }

            planet = new Planet(_chunkProvider, planetId);
            _planets.Add(planetId, planet);
            return planet;
        }
    }
}
