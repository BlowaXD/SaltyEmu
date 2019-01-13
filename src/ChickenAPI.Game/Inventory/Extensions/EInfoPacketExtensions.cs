using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class EInfoPacketExtensions
    {
        public static EInfoPacket GenerateEInfoPacket(this ItemInstanceDto itemInstance)
        {
            EquipmentType equipmentSlot = itemInstance.Item.EquipmentSlot;
            switch (itemInstance.Item.ItemType)
            {
                case ItemType.Weapon:
                    switch (equipmentSlot)
                    {
                        case EquipmentType.MainWeapon:
                            return GenerateEInfoPrimaryWeapon(itemInstance);
                        case EquipmentType.SecondaryWeapon:
                            return GenerateSecondaryWeapon(itemInstance);
                        default:
                            return null;
                    }

                case ItemType.Armor:
                    return GenerateEInfoArmor(itemInstance);

                case ItemType.Jewelery:
                    return GenerateEInfoJewelery(itemInstance);

                case ItemType.Specialist:
                    return GenerateEInfoSpecialist(itemInstance);

                default:
                    return null;
            }
        }

        private static EInfoPacket GenerateEInfoPrimaryWeapon(ItemInstanceDto itemInstance)
        {
            byte classe = itemInstance.Item.Class;
            switch (classe)
            {
                case (byte)ItemClassType.Unknown4:
                    return new EInfoPacket
                    {
                        EInfoType = EInfoPacketType.MainWeaponArcher,
                        ItemVNum = itemInstance.ItemId,
                        Rare = itemInstance.Rarity,
                        Upgrade = itemInstance.Upgrade,
                        Fixed = itemInstance.IsFixed,
                        LevelMinimum = itemInstance.Item.LevelMinimum,
                        CloseDefense = itemInstance.Item.DamageMinimum, // review this
                        RangeDefense = itemInstance.Item.DamageMaximum, // review this
                        MagicDefense = itemInstance.Item.HitRate, // review this
                        DefenseDodge = itemInstance.Item.CriticalLuckRate, // review this
                        CriticalRate = itemInstance.Item.CriticalRate, // review this
                        Ammo = itemInstance.Ammo,
                        MaximumAmmo = itemInstance.Item.MaximumAmmo,
                        Price = itemInstance.Item.Price,
                        Unknown2 = -1,
                        Rare2 = 0,
                        BoundCharacterId = 0,
                        ShellEffectCount = 0,
                        ShellEffect = null
                    };
                case (byte)ItemClassType.Unknown8:
                    return new EInfoPacket
                    {
                        EInfoType = EInfoPacketType.MainWeaponMagician,
                        ItemVNum = itemInstance.ItemId,
                        Rare = itemInstance.Rarity,
                        Upgrade = itemInstance.Upgrade,
                        Fixed = itemInstance.IsFixed,
                        LevelMinimum = itemInstance.Item.LevelMinimum,
                        CloseDefense = itemInstance.Item.DamageMinimum,
                        RangeDefense = itemInstance.Item.DamageMaximum,
                        MagicDefense = itemInstance.Item.HitRate,
                        DefenseDodge = itemInstance.Item.CriticalLuckRate,
                        CriticalRate = itemInstance.Item.CriticalRate,
                        Ammo = itemInstance.Ammo,
                        MaximumAmmo = itemInstance.Item.MaximumAmmo,
                        Price = itemInstance.Item.Price,
                        Unknown2 = -1,
                        Rare2 = 0,
                        BoundCharacterId = 0,
                        ShellEffectCount = 0,
                        ShellEffect = null
                    };
                default:
                    return new EInfoPacket
                    {
                        EInfoType = EInfoPacketType.WeaponDefault,
                        ItemVNum = itemInstance.ItemId,
                        Rare = itemInstance.Rarity,
                        Upgrade = itemInstance.Upgrade,
                        Fixed = itemInstance.IsFixed,
                        LevelMinimum = itemInstance.Item.LevelMinimum,
                        CloseDefense = itemInstance.Item.DamageMinimum,
                        RangeDefense = itemInstance.Item.DamageMaximum,
                        MagicDefense = itemInstance.Item.HitRate,
                        DefenseDodge = itemInstance.Item.CriticalLuckRate,
                        CriticalRate = itemInstance.Item.CriticalRate,
                        Ammo = itemInstance.Ammo,
                        MaximumAmmo = itemInstance.Item.MaximumAmmo,
                        Price = itemInstance.Item.Price,
                        Unknown2 = -1,
                        Rare2 = 0,
                        BoundCharacterId = 0,
                        ShellEffectCount = 0,
                        ShellEffect = null
                    };
            }
        }

        private static EInfoPacket GenerateSecondaryWeapon(ItemInstanceDto itemInstance)
        {
            byte classe = itemInstance.Item.Class;
            switch (classe)
            {
                case (byte)ItemClassType.Unknown1:
                case (byte)ItemClassType.Unknown2:
                    return new EInfoPacket
                    {
                        EInfoType = EInfoPacketType.MainWeaponArcher,
                        ItemVNum = itemInstance.ItemId,
                        Rare = itemInstance.Rarity,
                        Upgrade = itemInstance.Upgrade,
                        Fixed = itemInstance.IsFixed,
                        LevelMinimum = itemInstance.Item.LevelMinimum,
                        CloseDefense = itemInstance.Item.DamageMinimum,
                        RangeDefense = itemInstance.Item.DamageMaximum,
                        MagicDefense = itemInstance.Item.HitRate,
                        DefenseDodge = itemInstance.Item.CriticalLuckRate,
                        CriticalRate = itemInstance.Item.CriticalRate,
                        Ammo = itemInstance.Ammo,
                        MaximumAmmo = itemInstance.Item.MaximumAmmo,
                        Price = itemInstance.Item.Price,
                        Unknown2 = -1,
                        Rare2 = 0,
                        BoundCharacterId = 0,
                        ShellEffectCount = 0,
                        ShellEffect = null
                    };
                default:
                    return new EInfoPacket
                    {
                        EInfoType = EInfoPacketType.WeaponDefault,
                        ItemVNum = itemInstance.ItemId,
                        Rare = itemInstance.Rarity,
                        Upgrade = itemInstance.Upgrade,
                        Fixed = itemInstance.IsFixed,
                        LevelMinimum = itemInstance.Item.LevelMinimum,
                        CloseDefense = itemInstance.Item.DamageMinimum,
                        RangeDefense = itemInstance.Item.DamageMaximum,
                        MagicDefense = itemInstance.Item.HitRate,
                        DefenseDodge = itemInstance.Item.CriticalLuckRate,
                        CriticalRate = itemInstance.Item.CriticalRate,
                        Ammo = itemInstance.Ammo,
                        MaximumAmmo = itemInstance.Item.MaximumAmmo,
                        Price = itemInstance.Item.Price,
                        Unknown2 = -1,
                        Rare2 = 0,
                        BoundCharacterId = 0,
                        ShellEffectCount = 0,
                        ShellEffect = null
                    };
            }
        }

        private static EInfoPacket GenerateEInfoArmor(ItemInstanceDto itemInstance) =>
            new EInfoPacket
            {
                EInfoType = EInfoPacketType.Armor,
                ItemVNum = itemInstance.ItemId,
                Rare = itemInstance.Rarity,
                Upgrade = itemInstance.Upgrade,
                Fixed = itemInstance.IsFixed,
                LevelMinimum = itemInstance.Item.LevelMinimum,
                CloseDefense = itemInstance.Item.CloseDefence,
                RangeDefense = itemInstance.Item.DistanceDefence,
                MagicDefense = itemInstance.Item.MagicDefence,
                DefenseDodge = itemInstance.Item.DefenceDodge,
                Price = itemInstance.Item.Price,
                Unknown2 = -1,
                Rare2 = 0,
                BoundCharacterId = 0,
                ShellEffectCount = 0,
                ShellEffect = 0
            };

        private static EInfoPacket GenerateEInfoJewelery(ItemInstanceDto itemInstance)
        {
            EquipmentType equipmentSlot = itemInstance.Item.EquipmentSlot;
            switch (equipmentSlot)
            {
                case EquipmentType.Amulet:
                    return new EInfoPacket
                    {
                        EInfoType = EInfoPacketType.Jewelery,
                        ItemVNum = itemInstance.ItemId,
                        LevelMinimum = itemInstance.Item.LevelMinimum,
                        DurabilityPoint = 0, // TODO: Change this 0
                        Unknown = 100,
                        Unknown1 = 0,
                        Price = itemInstance.Item.Price
                    };
                case EquipmentType.Fairy:
                    return new EInfoPacket
                    {
                        EInfoType = EInfoPacketType.Jewelery,
                        ItemVNum = itemInstance.ItemId,
                        Element = itemInstance.Item.Element,
                        ElementRate = itemInstance.ElementRate,
                        Unknown = 0, // idk
                        Unknown1 = 0, // idk
                        Unknown2 = 0, // idk
                        Price = 0, // idk
                        Rare2 = 0 // idk
                    };
                default:
                    return new EInfoPacket
                    {
                        EInfoType = EInfoPacketType.Jewelery,
                        ItemVNum = itemInstance.ItemId,
                        LevelMinimum = itemInstance.Item.LevelMinimum,
                        ElementRate = itemInstance.Item.MaxCellonLvl, // review this
                        Unknown = itemInstance.Item.MaxCellon, // review this
                        Unknown1 = null, // Cellon Options
                        Unknown2 = 0, // idk
                        Price = itemInstance.Item.Price,
                        Rare2 = 0 // Cellon Info
                    };
            }
        }

        private static EInfoPacket GenerateEInfoSpecialist(ItemInstanceDto itemInstance) =>
            new EInfoPacket
            {
                EInfoType = EInfoPacketType.Specialist,
                ItemVNum = itemInstance.ItemId
            };
    }
}