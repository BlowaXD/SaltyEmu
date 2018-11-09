using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Entities
{
    [PacketHeader("drop")]
    public class DropPacket : PacketBase
    {
        [PacketIndex(0)]
        public long ItemVnum { get; set; }

        [PacketIndex(1)]
        public long TransportId { get; set; }

        [PacketIndex(2)]
        public short PositionX { get; set; }

        [PacketIndex(3)]
        public short PositionY { get; set; }

        [PacketIndex(4)]
        public long Quantity { get; set; }

        [PacketIndex(5)]
        public bool IsQuestDrop { get; set; }

        [PacketIndex(6)]
        public byte Unknown { get; set; } = 0;

        [PacketIndex(7)]
        public short Unknown2 { get; set; } = -1;
    }
}