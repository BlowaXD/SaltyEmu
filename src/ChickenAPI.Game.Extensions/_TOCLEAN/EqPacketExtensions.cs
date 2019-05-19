using System.Collections.Generic;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Inventory;
using ChickenAPI.Packets.ServerPackets.Visibility;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class EqPacketExtensions
    {
        public static InEquipmentSubPacket GenerateEqListInfoPacket(this InventoryComponent inventory) => new InEquipmentSubPacket
        {
            Hat = (short?)inventory.Wear[(int)EquipmentType.Hat]?.ItemId ?? -1,
            Armor = (short?)inventory.Wear[(int)EquipmentType.Armor]?.ItemId ?? -1,
            MainWeapon = (short?)inventory.Wear[(int)EquipmentType.MainWeapon]?.ItemId ?? -1,
            SecondaryWeapon = (short?)inventory.Wear[(int)EquipmentType.SecondaryWeapon]?.ItemId ?? -1,
            Mask = (short?)inventory.Wear[(int)EquipmentType.Mask]?.ItemId ?? -1,
            Fairy = (short?)inventory.Wear[(int)EquipmentType.Fairy]?.ItemId ?? -1,
            CostumeSuit = (short?)inventory.Wear[(int)EquipmentType.CostumeSuit]?.ItemId ?? -1,
            CostumeHat = (short?)inventory.Wear[(int)EquipmentType.CostumeHat]?.ItemId ?? -1,
            WeaponSkin = (short?)inventory.Wear[(int)EquipmentType.WeaponSkin]?.ItemId ?? -1
        };

        public static UpgradeRareSubPacket GenerateEqRareInfoPacket(this InventoryComponent inventory, EquipmentType type) => new UpgradeRareSubPacket
        {
            Upgrade = inventory.Wear[(int)type]?.Upgrade ?? 0,
            Rare = inventory.Wear[(int)type]?.Rarity ?? 0
        };

        public static EqPacket GenerateEqPacket(this IPlayerEntity player) => new EqPacket
        {
            VisualId = player.Id,
            Visibility = (byte)player.NameAppearance,
            Gender = player.Character.Gender,
            HairStyle = player.Character.HairStyle,
            Haircolor = player.Character.HairColor,
            ClassType = player.Character.Class,
            ArmorUpgradeRarePacket = player.Inventory.GenerateEqRareInfoPacket(EquipmentType.Armor),
            WeaponUpgradeRarePacket = player.Inventory.GenerateEqRareInfoPacket(EquipmentType.MainWeapon),
            EqSubPacket = player.Inventory.GenerateEqListInfoPacket(),
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
                Rare = (player.Weapon?.Rarity ?? 0)
            };
            var armor = new UpgradeRareSubPacket
            {
                Upgrade = player.Armor?.Upgrade ?? 0,
                Rare = (player.Armor?.Rarity ?? 0)
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