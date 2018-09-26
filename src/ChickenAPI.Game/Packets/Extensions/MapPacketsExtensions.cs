using System;
using ChickenAPI.Game.Maps;
using ChickenAPI.Packets.Game.Server.MiniMap;

namespace ChickenAPI.Game.Packets.Extensions
{
    public static class MapPacketsExtensions
    {
        public static CMapPacket GenerateCMapPacket(this IMap map) => new CMapPacket
        {
            Type = 0,
            Id = Convert.ToInt16(map.Id),
            MapType = 1
        };
    }
}