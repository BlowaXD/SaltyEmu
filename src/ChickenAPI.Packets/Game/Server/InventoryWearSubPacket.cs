﻿namespace ChickenAPI.Game.Packets.Game.Server
{
    [PacketHeader("eq_subpacket")]
    public class InventoryWearSubPacket : PacketBase
    {


        [PacketIndex(0)]
        public long Hat { get; set; }

        [PacketIndex(1, SeparatorBeforeProperty = ".")]
        public long Armor { get; set; }

        [PacketIndex(2, SeparatorBeforeProperty = ".")]
        public long MainWeapon { get; set; }

        [PacketIndex(3, SeparatorBeforeProperty = ".")]
        public long SecondaryWeapon { get; set; }

        [PacketIndex(4, SeparatorBeforeProperty = ".")]
        public long Mask { get; set; }

        [PacketIndex(5, SeparatorBeforeProperty = ".")]
        public long Fairy { get; set; }

        [PacketIndex(6, SeparatorBeforeProperty = ".")]
        public long CostumeSuit { get; set; }

        [PacketIndex(7, SeparatorBeforeProperty = ".")]
        public long CostumeHat { get; set; }

        [PacketIndex(8, SeparatorBeforeProperty = ".")]
        public long WeaponSkin { get; set; }
    }
}