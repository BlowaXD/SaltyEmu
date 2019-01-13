using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("e_info")]
    public class EInfoPacket : PacketBase
    {
        [PacketIndex(0)]
        public EInfoPacketType EInfoType { get; set; }

        [PacketIndex(1)]
        public long ItemVNum { get; set; }

        [PacketIndex(2, IsOptional = true)]
        public short? Rare { get; set; }

        [PacketIndex(3, IsOptional = true)]
        public short? Upgrade { get; set; }

        [PacketIndex(4, IsOptional = true)]
        public bool? Fixed { get; set; }

        [PacketIndex(5)]
        public byte LevelMinimum { get; set; }

        [PacketIndex(13, IsOptional = true)]
        public short? DurabilityPoint { get; set; }

        [PacketIndex(14, IsOptional = true)]
        public int? Unknown { get; set; } // default 100 for Amulets

        [PacketIndex(15, IsOptional = true)]
        public int? Unknown1 { get; set; } // default 0 for Amulets

        [PacketIndex(16, IsOptional = true)]
        public byte? Element { get; set; }

        [PacketIndex(17, IsOptional = true)]
        public short? ElementRate { get; set; }

        [PacketIndex(18, IsOptional = true)]
        public long? Price { get; set; }

        [PacketIndex(19, IsOptional = true)]
        public int? Unknown2 { get; set; } // default -1 for Weapons and Armors

        [PacketIndex(20, IsOptional = true)]
        public short Rare2 { get; set; }

        [PacketIndex(21, IsOptional = true)]
        public long BoundCharacterId { get; set; }

        [PacketIndex(22, IsOptional = true)]
        public byte? ShellEffectCount { get; set; } // Review this

        [PacketIndex(23, IsOptional = true)]
        public byte? ShellEffect { get; set; } // Review this

        #region Armor

        [PacketIndex(6, IsOptional = true)]
        public short? CloseDefense { get; set; }

        [PacketIndex(7, IsOptional = true)]
        public short? RangeDefense { get; set; }

        [PacketIndex(8, IsOptional = true)]
        public short? MagicDefense { get; set; }

        [PacketIndex(9, IsOptional = true)]
        public short? DefenseDodge { get; set; }

        [PacketIndex(10, IsOptional = true)]
        public short? CriticalRate { get; set; }

        #endregion

        #region Weapon

        [PacketIndex(11, IsOptional = true)]
        public byte? Ammo { get; set; }

        [PacketIndex(12, IsOptional = true)]
        public byte? MaximumAmmo { get; set; }

        #endregion
    }
}