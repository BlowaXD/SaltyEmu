using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.MiniMap
{
    [PacketHeader("c_map")]
    public class CMapPacket : PacketBase
    {
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