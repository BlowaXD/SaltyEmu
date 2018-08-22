using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("pairy")]
    public class PairyPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long CharacterId { get; set; }

        [PacketIndex(2)]
        public byte FairyMoveType { get; set; }

        [PacketIndex(3)]
        public ElementType ElementType { get; set; }

        [PacketIndex(4)]
        public short FairyLevel { get; set; }

        [PacketIndex(5)]
        public short ItemMorph { get; set; }
    }
}