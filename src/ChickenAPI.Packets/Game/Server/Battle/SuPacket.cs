using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Battle
{
    [PacketHeader("su")]
    public class SuPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public VisualType TargetVisualType { get; set; }

        [PacketIndex(3)]
        public long TargetId { get; set; }

        [PacketIndex(4)]
        public long SkillVnum { get; set; }

        [PacketIndex(5)]
        public long SkillCooldown { get; set; }

        [PacketIndex(6)]
        public long AttackAnimation { get; set; }

        [PacketIndex(7)]
        public long SkillEffect { get; set; }

        [PacketIndex(8)]
        public long PositionX { get; set; }

        [PacketIndex(9)]
        public long PositionY { get; set; }

        [PacketIndex(10)]
        public bool TargetIsAlive { get; set; }

        [PacketIndex(11)]
        public byte HpPercentage { get; set; }

        [PacketIndex(12)]
        public uint Damage { get; set; }

        [PacketIndex(13)]
        public SuPacketHitMode HitMode { get; set; }

        [PacketIndex(14)]
        public int SkillTypeMinusOne { get; set; }
    }
}