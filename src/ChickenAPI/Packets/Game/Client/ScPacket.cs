﻿namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("sc")]
    public class ScPacket : PacketBase
    {
        [PacketIndex(0)]
        public byte Type { get; set; }

        [PacketIndex(1)]
        public byte MainWeaponUpgrade { get; set; }

        [PacketIndex(2)]
        public int MinHit { get; set; }

        [PacketIndex(3)]
        public int MaxHit { get; set; }

        [PacketIndex(4)]
        public byte HitRate { get; set; }

        [PacketIndex(5)]
        public byte CriticalHitRate { get; set; }

        [PacketIndex(6)]
        public byte CriticalHitMultiplier { get; set; }

        [PacketIndex(7)]
        public byte Type2 { get; set; }

        [PacketIndex(8)]
        public short SecondaryWeaponUpgrade { get; set; }

        [PacketIndex(9)]
        public short SecondaryMinHit { get; set; }

        [PacketIndex(10)]
        public short SecondaryMaxHit { get; set; }

        [PacketIndex(11)]
        public short SecondaryHitRate { get; set; }

        [PacketIndex(12)]
        public byte SecondaryCriticalHitRate { get; set; }

        [PacketIndex(13)]
        public short SecondaryCriticalHitMultiplier { get; set; }

        [PacketIndex(14)]
        public byte ArmorUpgrade { get; set; }

        [PacketIndex(15)]
        public short Defence { get; set; }

        [PacketIndex(16)]
        public short DefenceRate { get; set; }

        [PacketIndex(17)]
        public short DistanceDefence { get; set; }

        [PacketIndex(18)]
        public short DistanceDefenceRate { get; set; }

        [PacketIndex(19)]
        public short MagicalDefence { get; set; }

        [PacketIndex(20)]
        public short FireResistance { get; set; }

        [PacketIndex(21)]
        public short WaterResistance { get; set; }

        [PacketIndex(22)]
        public short LightResistance { get; set; }

        [PacketIndex(23)]
        public short DarkResistance { get; set; }
}
}
