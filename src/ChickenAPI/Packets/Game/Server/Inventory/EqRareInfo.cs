using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Components;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("subpacket_eq_rare_info")]
    public class EqRareInfo : PacketBase
    {

        public EqRareInfo(InventoryComponent inventory)
        {
            WeaponUpgrade = inventory.Wear[(int)EquipmentType.MainWeapon]?.Upgrade ?? 0;
            WeaponRarity = (sbyte)(inventory.Wear[(int)EquipmentType.MainWeapon]?.Rarity ?? 0);
            ArmorUpgrade = inventory.Wear[(int)EquipmentType.Armor]?.Upgrade ?? 0;
            ArmorRarity = (sbyte)(inventory.Wear[(int)EquipmentType.MainWeapon]?.Rarity ?? 0);
        }

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