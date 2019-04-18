using System;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class InventoryUseExtension
    {
        public static void UseItem(this InventoryComponent inventory, ItemInstanceDto itemInstance)
        {
            switch (itemInstance.Item.Type)
            {
                case InventoryType.Wear:
                    inventory.UseWear(itemInstance);
                    break;
            }
        }

        public static void UseWear(this InventoryComponent inventory, ItemInstanceDto itemInstance)
        {
            switch (itemInstance.Item.EquipmentSlot)
            {
                case EquipmentType.MainWeapon:
                    break;
                case EquipmentType.Armor:
                    break;
                case EquipmentType.Hat:
                    break;
                case EquipmentType.Gloves:
                    break;
                case EquipmentType.Boots:
                    break;
                case EquipmentType.SecondaryWeapon:
                    break;
                case EquipmentType.Necklace:
                    break;
                case EquipmentType.Ring:
                    break;
                case EquipmentType.Bracelet:
                    break;
                case EquipmentType.Mask:
                    break;
                case EquipmentType.Fairy:
                    break;
                case EquipmentType.Amulet:
                    break;
                case EquipmentType.Sp:
                    break;
                case EquipmentType.CostumeSuit:
                    break;
                case EquipmentType.CostumeHat:
                    break;
                case EquipmentType.WeaponSkin:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}