using System.Text;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace ChickenAPI.Game.Inventory.Extensions
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
            WeaponSkin = inventory.Wear[(int)EquipmentType.WeaponSkin]?.ItemId ?? -1
        };

        public static EqRareInfo GenerateEqRareInfoPacket(this InventoryComponent inventory) => new EqRareInfo
        {
            WeaponUpgrade = inventory.Wear[(int)EquipmentType.MainWeapon]?.Upgrade ?? 0,
            WeaponRarity = (sbyte)(inventory.Wear[(int)EquipmentType.MainWeapon]?.Rarity ?? 0),
            ArmorUpgrade = inventory.Wear[(int)EquipmentType.Armor]?.Upgrade ?? 0,
            ArmorRarity = (sbyte)(inventory.Wear[(int)EquipmentType.MainWeapon]?.Rarity ?? 0)
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
            EqInfo = player.Inventory.GenerateEqRareInfoPacket()
        };

        public static EquipmentPacket GenerateEquipmentPacket(this IPlayerEntity player)
        {
            var tmp = new StringBuilder();
            ItemInstanceDto[] subInventory = player.Inventory.Wear;

            for (int i = 0; i < subInventory.Length; i++)
            {
                ItemInstanceDto item = subInventory[i];
                if (item == null)
                {
                    continue;
                }

                tmp.Append(' ');
                tmp.Append(i);
                tmp.Append('.');
                tmp.Append(item.ItemId);
                tmp.Append('.');
                tmp.Append(item.Rarity);
                tmp.Append('.');
                tmp.Append(item.Item.IsColored ? item.Design : item.Upgrade);
                tmp.Append(".0");
            }

            return new EquipmentPacket
            {
                EqRare = player.Inventory.GenerateEqRareInfoPacket(),
                EqList = tmp.ToString().Trim()
            };
        }
    }
}