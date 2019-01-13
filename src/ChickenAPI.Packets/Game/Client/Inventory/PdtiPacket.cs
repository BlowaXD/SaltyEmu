using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Inventory
{
    [PacketHeader("pdti")]
    public class PdtiPacket : PacketBase
    {
        [PacketIndex(0)]
        public short Unknow { get; set; }

        [PacketIndex(1)]
        public long ItemVnum { get; set; }

        [PacketIndex(2)]
        public short Unknow2 { get; set; }

        [PacketIndex(3)]
        public short Unknow3 { get; set; }

        [PacketIndex(4)]
        public short ItemUpgrade { get; set; }

        [PacketIndex(5)]
        public short Unknow4 { get; set; }
    }
}