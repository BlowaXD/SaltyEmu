using System;
using System.Collections.Generic;
using System.Text;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.UI
{
    [PacketHeader("e_info")]
    public class EInfoPacket : PacketBase
    {
        [PacketIndex(0)]
        public short Unknown1 { get; set; }

        [PacketIndex(1)]
        public long MonsterVnum { get; set; }

        [PacketIndex(2)]
        public byte Level { get; set; }

        [PacketIndex(3)]
        public ElementType Element { get; set; }

        [PacketIndex(4)]
        public byte AttackClass { get; set; }

        [PacketIndex(5)]
        public short ElementRate { get; set; }

        [PacketIndex(6)]
        public byte AttackUpgrade { get; set; }

        [PacketIndex(7)]
        public short DamageMinimum { get; set; }

        [PacketIndex(8)]
        public short DamageMaximum { get; set; }

        [PacketIndex(9)]
        public short Concentrate { get; set; }

        [PacketIndex(10)]
        public byte CriticalChance { get; set; }

        [PacketIndex(11)]
        public short CriticalRate { get; set; }

        [PacketIndex(12)]
        public byte DefenceUpgrade { get; set; }

        [PacketIndex(13)]
        public short CloseDefence { get; set; }

        [PacketIndex(14)]
        public short DefenceDodge { get; set; }

        [PacketIndex(15)]
        public short DistanceDefence { get; set; }

        [PacketIndex(16)]
        public short DistanceDefenceDodge { get; set; }

        [PacketIndex(17)]
        public short MagicDefence { get; set; }

        [PacketIndex(18)]
        public sbyte FireResistance { get; set; }

        [PacketIndex(19)]
        public sbyte WaterResistance { get; set; }

        [PacketIndex(20)]
        public sbyte LightResistance { get; set; }

        [PacketIndex(21)]
        public sbyte DarkResistance { get; set; }

        [PacketIndex(22)]
        public int MaxHp { get; set; }

        [PacketIndex(23)]
        public int MaxMp { get; set; }

        [PacketIndex(24)]
        public short Unknown2 { get; set; }

        [PacketIndex(25)]
        public string Name { get; set; }
    }
}
