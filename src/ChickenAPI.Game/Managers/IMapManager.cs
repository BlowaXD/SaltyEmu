using System.Collections.Generic;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Managers
{
    public interface IMapManager
    {
        IReadOnlyDictionary<long, IMap> Maps { get; }

        IMapLayer GetBaseMapLayer(long mapId);
        IMapLayer GetBaseMapLayer(IMap map);
    }
}