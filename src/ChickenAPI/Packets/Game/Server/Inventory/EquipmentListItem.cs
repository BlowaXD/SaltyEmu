using ChickenAPI.Enums.Game.Items;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("subpacket_eq_list")]
    public class EquipmentListItem
    {
        [PacketIndex(0)]
        public EquipmentType EquipmentType { get; set; }

        [PacketIndex(1, SeparatorBeforeProperty = ".")]
        public long ItemId { get; set; }

        [PacketIndex(2, SeparatorBeforeProperty = ".")]
        public sbyte ItemRarity { get; set; }

        [PacketIndex(3, SeparatorBeforeProperty = ".")]
        public byte DesignOrUpgrade { get; set; }

        [PacketIndex(4, SeparatorBeforeProperty = ".")]
        public byte Unknown { get; set; } = 0;
    }
}