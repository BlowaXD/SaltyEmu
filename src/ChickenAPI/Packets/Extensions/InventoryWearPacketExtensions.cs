using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Inventory;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace ChickenAPI.Game.Packets.Extensions
{
    public static class InventoryWearPacketExtensions
    {
        public static InventoryWearSubPacket GenerateInventoryWearPacket(this IPlayerEntity player)
        {
            InventoryComponent inventory = player.Inventory;
            return new InventoryWearSubPacket
            {
                Hat = inventory.Wear[(int)EquipmentType.Hat]?.ItemId ?? -1,
                Armor = inventory.Wear[(int)EquipmentType.Armor]?.ItemId ?? -1,
                MainWeapon = inventory.Wear[(int)EquipmentType.MainWeapon]?.ItemId ?? -1,
                SecondaryWeapon = inventory.Wear[(int)EquipmentType.SecondaryWeapon]?.ItemId ?? -1,
                Mask = inventory.Wear[(int)EquipmentType.Mask]?.ItemId ?? -1,
                Fairy = inventory.Wear[(int)EquipmentType.Fairy]?.ItemId ?? -1,
                CostumeSuit = inventory.Wear[(int)EquipmentType.CostumeSuit]?.ItemId ?? -1,
                CostumeHat = inventory.Wear[(int)EquipmentType.CostumeHat]?.ItemId ?? -1,
                WeaponSkin = inventory.Wear[(int)EquipmentType.WeaponSkin]?.ItemId ?? -1
            };
        }
    }
}