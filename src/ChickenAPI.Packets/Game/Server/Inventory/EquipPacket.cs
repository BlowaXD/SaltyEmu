using System.Collections.Generic;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("equip")]
    public class EquipPacket : PacketBase
    {
        [PacketIndex(0, RemoveSeparator = true)]
        public EqRareInfo EqRare { get; set; }


        [PacketIndex(1, SeparatorNestedElements = " ", IsOptional = true)]
        public List<EquipSubPacket> EqList { get; set; }

        [PacketHeader("equip_subpacket")]
        public class EquipSubPacket : PacketBase
        {
            [PacketIndex(0)]
            public int WearIndex { get; set; }

            [PacketIndex(1, SeparatorBeforeProperty = ".")]
            public long ItemId { get; set; }

            [PacketIndex(2, SeparatorBeforeProperty = ".")]
            public byte ItemRarity { get; set; }

            [PacketIndex(3, SeparatorBeforeProperty = ".")]
            public byte UpgradeOrDesign { get; set; }

            [PacketIndex(4, SeparatorBeforeProperty = ".")]
            public byte Unknown { get; set; }
        }
    }
}