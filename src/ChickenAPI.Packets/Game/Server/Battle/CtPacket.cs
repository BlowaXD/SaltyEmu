﻿using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Battle
{
    [PacketHeader("ct")]
    public class CtPacket : PacketBase
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
        public long CastAnimationId { get; set; }

        [PacketIndex(5)]
        public long CastEffect { get; set; }

        [PacketIndex(6)]
        public long SkillId { get; set; }
    }
}