using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Battle
{
    [PacketHeader("rc")]
    public class RcPacket
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public long HealAmount { get; set; }

        [PacketIndex(3)]
        public long Unknown { get; set; }
    }
}