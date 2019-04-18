using System.Collections.Generic;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Inventory;

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
            WeaponRarity = inventory.Wear[(int)EquipmentType.MainWeapon]?.Rarity ?? 0,
            ArmorUpgrade = inventory.Wear[(int)EquipmentType.Armor]?.Upgrade ?? 0,
            ArmorRarity = inventory.Wear[(int)EquipmentType.MainWeapon]?.Rarity ?? 0
        };

        public static EqPacket GenerateEqPacket(this IPlayerEntity player) => new EqPacket
        {
            VisualId = player.Id,
            NameAppearance = player.NameAppearance,
            Gender = player.Character.Gender,
            HairStyle = player.Character.HairStyle,
            Haircolor = player.Character.HairColor,
            CharacterClassType = player.Character.Class,
            EqList = player.Inventory.GenerateEqListInfoPacket(),
            EqInfo = player.Inventory.GenerateEqRareInfoPacket()
        };

        /// <summary>
        /// This packet is used for character items equiped in "P" panel (wear items)
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static EquipPacket GenerateEquipPacket(this IPlayerEntity player)
        {
            EquipmentSubPacket generateEquipmentSubPacket(EquipmentType eqType)
            {
                ItemInstanceDto eq = player.Inventory.Equipment[((short)eqType)];
                if (eq == null)
                {
                    return null;
                }

                return new EquipmentSubPacket
                {
                    EquipmentType = eqType,
                    VNum = (short)eq.ItemId,
                    Rare = eq.Rarity,
                    Upgrade = (eq?.Item.IsColored == true ? eq?.Design : eq?.Upgrade) ?? 0,
                    Unknown = 0,
                };
            }

            List<EquipmentSubPacket> tmp = new List<EquipmentSubPacket>();
            ItemInstanceDto[] subInventory = player.Inventory.Wear;

            for (int i = 0; i < subInventory.Length; i++)
            {
                ItemInstanceDto item = subInventory[i];
                if (item == null)
                {
                    continue;
                }

                tmp.Add(new EquipmentSubPacket()
                {
                    EquipmentType = (EquipmentType)i,
                    VNum = (short)item.ItemId,
                    Rare = (byte)item.Rarity,
                    Upgrade = item.Item.IsColored ? item.Design : item.Upgrade,
                    Unknown = 0
                });
            }

            var weapon = new UpgradeRareSubPacket
            {
                Upgrade = player.Weapon?.Upgrade ?? 0,
                Rare = (byte)(player.Weapon?.Rarity ?? 0)
            };
            var armor = new UpgradeRareSubPacket
            {
                Upgrade = player.Armor?.Upgrade ?? 0,
                Rare = (byte)(player.Armor?.Rarity ?? 0)
            };

            return new EquipPacket
            {
                WeaponUpgradeRareSubPacket = weapon,
                ArmorUpgradeRareSubPacket = armor,
                Armor = generateEquipmentSubPacket(EquipmentType.Armor),
                WeaponSkin = generateEquipmentSubPacket(EquipmentType.WeaponSkin),
                SecondaryWeapon = generateEquipmentSubPacket(EquipmentType.SecondaryWeapon),
                Sp = generateEquipmentSubPacket(EquipmentType.Sp),
                Amulet = generateEquipmentSubPacket(EquipmentType.Amulet),
                Boots = generateEquipmentSubPacket(EquipmentType.Boots),
                CostumeHat = generateEquipmentSubPacket(EquipmentType.CostumeHat),
                CostumeSuit = generateEquipmentSubPacket(EquipmentType.CostumeSuit),
                Fairy = generateEquipmentSubPacket(EquipmentType.Fairy),
                Gloves = generateEquipmentSubPacket(EquipmentType.Gloves),
                Hat = generateEquipmentSubPacket(EquipmentType.Hat),
                MainWeapon = generateEquipmentSubPacket(EquipmentType.MainWeapon),
                Mask = generateEquipmentSubPacket(EquipmentType.Mask),
                Necklace = generateEquipmentSubPacket(EquipmentType.Necklace),
                Ring = generateEquipmentSubPacket(EquipmentType.Ring),
                Bracelet = generateEquipmentSubPacket(EquipmentType.Bracelet),
            };
        }
    }
}