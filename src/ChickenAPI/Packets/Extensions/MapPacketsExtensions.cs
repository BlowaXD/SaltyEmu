using System;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Packets.Extensions
{
    public static class MapPacketsExtensions
    {
        public static CMapPacket GenerateCMapPacket(this IMap map)
        {
            return new CMapPacket
            {
                Type = 0,
                Id = Convert.ToInt16(map.Id),
                MapType = 1,
            };
        }
    }
}