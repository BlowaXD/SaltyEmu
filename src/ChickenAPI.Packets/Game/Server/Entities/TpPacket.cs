using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Entities
{
    [PacketHeader("tp")]
    public class TpPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public short X { get; set; }

        [PacketIndex(3)]
        public short Y { get; set; }

        [PacketIndex(4)]
        public byte Unknown { get; set; }
    }
}