using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Components;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("subpacket_eq_list_info")]
    public class EqListInfo : PacketBase
    {
        public EqListInfo(InventoryComponent inventory)
        {
            Hat = inventory.Wear[(int)EquipmentType.Hat]?.ItemId ?? -1;
            Armor = inventory.Wear[(int)EquipmentType.Armor]?.ItemId ?? -1;
            MainWeapon = inventory.Wear[(int)EquipmentType.MainWeapon]?.ItemId ?? -1;
            SecondaryWeapon = inventory.Wear[(int)EquipmentType.SecondaryWeapon]?.ItemId ?? -1;
            Mask = inventory.Wear[(int)EquipmentType.Mask]?.ItemId ?? -1;
            Fairy = inventory.Wear[(int)EquipmentType.Fairy]?.ItemId ?? -1;
            CostumeSuit = inventory.Wear[(int)EquipmentType.CostumeSuit]?.ItemId ?? -1;
            CostumeHat = inventory.Wear[(int)EquipmentType.CostumeHat]?.ItemId ?? -1;
            WeaponSkin = inventory.Wear[(int)EquipmentType.WeaponSkin]?.ItemId ?? -1;
        }

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