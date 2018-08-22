using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("subpacket_eq_rare_info")]
    public class EqRareInfo : PacketBase
    {
        [PacketIndex(0)]
        public byte WeaponUpgrade { get; set; }

        [PacketIndex(1, SeparatorBeforeProperty = "")]
        public sbyte WeaponRarity { get; set; }

        [PacketIndex(2, SeparatorBeforeProperty = " ")]
        public byte ArmorUpgrade { get; set; }

        [PacketIndex(3, SeparatorBeforeProperty = "")]
        public sbyte ArmorRarity { get; set; }
    }
}