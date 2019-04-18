using System;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Packets.Game.Server.MiniMap;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class MapPacketsExtensions
    {
        public static CMapPacket GenerateCMapPacket(this IPlayerEntity player) => player.CurrentMap?.Map.GenerateCMapPacket();

        public static CMapPacket GenerateCMapPacket(this IMap map) => new CMapPacket
        {
            Type = 0,
            Id = Convert.ToInt16(map.Id),
            MapType = 1
        };
    }
}