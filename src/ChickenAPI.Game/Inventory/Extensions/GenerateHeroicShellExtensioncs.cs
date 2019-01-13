using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class GenerateHeroicShellExtensioncs
    {
        private static IRandomGenerator Random => new Lazy<IRandomGenerator>(() => ChickenContainer.Instance.Resolve<IRandomGenerator>()).Value;

        public static void GenerateHeroicShell(this ItemInstanceDto item, RarifyProtection protection)
        {
            byte shellType;
            if (protection != RarifyProtection.RandomHeroicAmulet)
            {
                return;
            }

            if (!item.Item.IsHeroic || item.Rarity <= 0)
            {
                return;
            }

            if (item.Rarity < 8)
            {
                shellType = (byte)(item.Item.ItemType == ItemType.Armor ? 11 : 10);
                if (shellType != 11 && shellType != 10)
                {
                }
            }
            else
            {
                List<byte> possibleTypes = new List<byte> { 4, 5, 6, 7 };
                int probability = (int)Random.Next();
                shellType = (byte)(item.Item.ItemType == ItemType.Armor
                    ? probability > 50 ? 5 : 7
                    : probability > 50
                        ? 4
                        : 6);
                if (!possibleTypes.Contains(shellType))
                {
                }
            }

            // item.EquipmentOptions.Clear();
            // int shellLevel = item.Item.LevelMinimum == 25 ? 101 : 106;
            // item.EquipmentOptions.AddRange(ShellGeneratorHelper.Instance.GenerateShell(shellType, item.Rarity == 8 ? 7 : item.Rarity, shellLevel));
        }
    }
}