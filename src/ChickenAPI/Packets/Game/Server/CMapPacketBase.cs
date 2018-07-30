using System;
using ChickenAPI.Game.Maps;

namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("c_map")]
    public class CMapPacketBase : PacketBase
    {
        public CMapPacketBase(IMap map)
        {
            Type = 0;
            Id = Convert.ToInt16(map.Id);
            MapType = 1;
        }

        public CMapPacketBase(byte type, short id, byte mapType)
        {
            Type = type;
            Id = id;
            MapType = mapType;
        }

        #region Properties
        
        [PacketIndex(0)]
        public byte Type { get; set; } // Seems to be always equal to 0

        [PacketIndex(1)]
        public short Id { get; set; }
        
        [PacketIndex(2)]
        public byte MapType { get; set; } // depends on the maptype (1 = base & 0 = instanciated I think)

        #endregion
    }
}
