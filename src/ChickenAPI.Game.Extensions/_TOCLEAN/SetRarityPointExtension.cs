using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class SetRarityPointExtension
    {
        private static IRandomGenerator Random => new Lazy<IRandomGenerator>(() => ChickenContainer.Instance.Resolve<IRandomGenerator>()).Value;

        public static void SetRarityPoint(this ItemInstanceDto item)
        {
            switch (item.Item.EquipmentSlot)
            {
                case EquipmentType.MainWeapon:
                case EquipmentType.SecondaryWeapon:
                {
                    int point = RarityPoint(item.Rarity, item.Item.IsHeroic ? (short)(95 + item.Item.LevelMinimum) : item.Item.LevelMinimum);
                    item.Concentration = 0;
                    item.HitRate = 0;
                    item.DamageMinimum = 0;
                    item.DamageMaximum = 0;
                    if (item.Rarity >= 0)
                    {
                        for (int i = 0; i < point; i++)
                        {
                            int rndn = Random.Next(0, 3);
                            if (rndn == 0)
                            {
                                item.Concentration++;
                                item.HitRate++;
                            }
                            else
                            {
                                item.DamageMinimum++;
                                item.DamageMaximum++;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i > item.Rarity * 10; i--)
                        {
                            item.DamageMinimum--;
                            item.DamageMaximum--;
                        }
                    }
                }
                    break;

                case EquipmentType.Armor:
                {
                    int point = RarityPoint(item.Rarity, item.Item.IsHeroic ? (short)(95 + item.Item.LevelMinimum) : item.Item.LevelMinimum);
                    item.CloseDodge = 0;
                    item.RangeDodge = 0;
                    item.RangeDefense = 0;
                    item.MagicDefense = 0;
                    item.CloseDefense = 0;
                    if (item.Rarity >= 0)
                    {
                        for (int i = 0; i < point; i++)
                        {
                            int rndn = Random.Next(0, 3);
                            if (rndn == 0)
                            {
                                item.CloseDodge++;
                                item.RangeDodge++;
                            }
                            else
                            {
                                item.RangeDefense++;
                                item.MagicDefense++;
                                item.CloseDefense++;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i > item.Rarity * 10; i--)
                        {
                            item.RangeDefense--;
                            item.MagicDefense--;
                            item.CloseDefense--;
                        }
                    }
                }
                    break;
            }
        }

        public static int RarityPoint(short rarity, short lvl)
        {
            int p;
            switch (rarity)
            {
                case 0:
                    p = 0;
                    break;

                case 1:
                    p = 1;
                    break;

                case 2:
                    p = 2;
                    break;

                case 3:
                    p = 3;
                    break;

                case 4:
                    p = 4;
                    break;

                case 5:
                    p = 5;
                    break;

                case 6:
                    p = 7;
                    break;

                case 7:
                    p = 10;
                    break;

                case 8:
                    p = 15;
                    break;

                default:
                    p = rarity * 2;
                    break;
            }

            return p * (lvl / 5 + 1);
        }
    }
}