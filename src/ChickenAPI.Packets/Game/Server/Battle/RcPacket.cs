using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Battle
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