using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client._NotYetSorted
{
    [PacketHeader("mlobj")]
    public class MlobjPacket : PacketBase
    {
        [PacketIndex(0)]
        public bool Deleted { get; set; }

        [PacketIndex(1)]
        public short Slot { get; set; }

        [PacketIndex(2)]
        public short MapX { get; set; }

        [PacketIndex(3)]
        public short MapY { get; set; }

        [PacketIndex(4)]
        public short Width { get; set; }

        [PacketIndex(5)]
        public short Height { get; set; }

        [PacketIndex(6)]
        public byte Unknown { get; set; } // looks like always 0

        [PacketIndex(7)]
        public short DurabilityPoint { get; set; }

        [PacketIndex(8)]
        public byte Unknown2 { get; set; } // looks like always 0

        [PacketIndex(9)]
        public bool IsWarehouse { get; set; }
    }
}