using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Battle
{
    [PacketHeader("ct")]
    public class CtPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public byte Unknown { get; set; } = 1;

        [PacketIndex(3)]
        public long SenderId { get; set; }

        [PacketIndex(4)]
        public long CastAnimationId { get; set; }

        [PacketIndex(5)]
        public long CastEffect { get; set; }

        [PacketIndex(6)]
        public long SkillVnum { get; set; }
    }
}