using System;

namespace Aster.Client.World
{
    public interface IPlanetProvider
    {
        Planet GetPlanet(Guid planetId);
    }
}
