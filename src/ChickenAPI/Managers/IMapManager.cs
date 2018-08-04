using System;
using System.Collections.Generic;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Maps;

namespace ChickenAPI.Game.Managers
{
    public interface IMapManager
    {
        IReadOnlyDictionary<long, IMap> Maps { get; }

        void ChangeMap(IPlayerEntity player, long mapId);
        void ChangeMapLayer(IPlayerEntity player, Guid mapLayerId);
        void ChangeMapLayer(IPlayerEntity player, IMapLayer layer);

        IMapLayer GetBaseMapLayer(long mapId);
        IMapLayer GetBaseMapLayer(IMap map);
    }
}