using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Battle
{
    [PacketHeader("bf")]
    public class BfPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public long ChargeValue { get; set; }

        [PacketIndex(3, SeparatorBeforeProperty = ".")]
        public long BuffId { get; set; }

        [PacketIndex(4, SeparatorBeforeProperty = ".")]
        public long Duration { get; set; }

        [PacketIndex(5)]
        public long BuffLevel { get; set; }
    }
}