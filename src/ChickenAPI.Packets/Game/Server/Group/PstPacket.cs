﻿using System.Collections.Generic;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Group
{
    [PacketHeader("pst")]
    public class PstPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public long GroupIndex { get; set; }

        [PacketIndex(3)]
        public int HpPercentage { get; set; }

        [PacketIndex(4)]
        public int MpPercentage { get; set; }

        [PacketIndex(5)]
        public int HpMax { get; set; }

        [PacketIndex(6)]
        public int MpMax { get; set; }

        [PacketIndex(7)]
        public GenderType Gender { get; set; }

        [PacketIndex(8)]
        public CharacterClassType Class { get; set; }

        [PacketIndex(9)]
        public int Morph { get; set; }

        [PacketIndex(10, SeparatorNestedElements = " ")]
        public List<long> Buffs { get; set; }
    }
}