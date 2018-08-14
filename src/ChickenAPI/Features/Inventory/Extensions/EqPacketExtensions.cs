using System.Collections.Generic;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Packets.Game.Client;
using ChickenAPI.Game.Packets.Game.Server.Inventory;
using EquipmentInfoPacket = ChickenAPI.Game.Packets.Game.Server.Inventory.EquipmentInfoPacket;

namespace ChickenAPI.Game.Features.Inventory.Extensions
{
    public static class EqPacketExtensions
    {
        public static EqListInfo GenerateEqListInfoPacket(this InventoryComponent inventory) => new EqListInfo
        {
            Hat = inventory.Wear[(int)EquipmentType.Hat]?.ItemId ?? -1,
            Armor = inventory.Wear[(int)EquipmentType.Armor]?.ItemId ?? -1,
            MainWeapon = inventory.Wear[(int)EquipmentType.MainWeapon]?.ItemId ?? -1,
            SecondaryWeapon = inventory.Wear[(int)EquipmentType.SecondaryWeapon]?.ItemId ?? -1,
            Mask = inventory.Wear[(int)EquipmentType.Mask]?.ItemId ?? -1,
            Fairy = inventory.Wear[(int)EquipmentType.Fairy]?.ItemId ?? -1,
            CostumeSuit = inventory.Wear[(int)EquipmentType.CostumeSuit]?.ItemId ?? -1,
            CostumeHat = inventory.Wear[(int)EquipmentType.CostumeHat]?.ItemId ?? -1,
            WeaponSkin = inventory.Wear[(int)EquipmentType.WeaponSkin]?.ItemId ?? -1,
        };

        public static EqRareInfo GenerateEqRareInfoPacket(this InventoryComponent inventory) => new EqRareInfo
        {
            WeaponUpgrade = inventory.Wear[(int)EquipmentType.MainWeapon]?.Upgrade ?? 0,
            WeaponRarity = (sbyte)(inventory.Wear[(int)EquipmentType.MainWeapon]?.Rarity ?? 0),
            ArmorUpgrade = inventory.Wear[(int)EquipmentType.Armor]?.Upgrade ?? 0,
            ArmorRarity = (sbyte)(inventory.Wear[(int)EquipmentType.MainWeapon]?.Rarity ?? 0),
        };

        public static EqPacket GenerateEqPacket(this IPlayerEntity player) => new EqPacket
        {
            CharacterId = player.Character.Id,
            VisualType = 0,
            GenderType = player.Character.Gender,
            HairStyleType = player.Character.HairStyle,
            HairColorType = player.Character.HairColor,
            CharacterClassType = player.Character.Class,
            EqList = player.Inventory.GenerateEqListInfoPacket(),
            EqInfo = player.Inventory.GenerateEqRareInfoPacket(),
        };

        public static EquipmentPacket GenerateEquipmentPacket(this IPlayerEntity player)
        {
            return new EquipmentPacket
            {
                EqRare = player.Inventory.GenerateEqRareInfoPacket(),
                EqList = new List<EquipmentInfoPacket>()
            };
        }
    }
}