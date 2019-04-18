using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;

namespace ChickenAPI.Game.Helpers
{
    public class CellonGeneratorHelper
    {
        private readonly Dictionary<int, Dictionary<JewelOptionType, CellonGenerator>> _generatorDictionary =
            new Dictionary<int, Dictionary<JewelOptionType, CellonGenerator>>
            {
                {
                    1,
                    new Dictionary<JewelOptionType, CellonGenerator>
                    {
                        { JewelOptionType.Hp, new CellonGenerator { Min = 30, Max = 100 } },
                        { JewelOptionType.Mp, new CellonGenerator { Min = 50, Max = 120 } },
                        { JewelOptionType.HpRecovery, new CellonGenerator { Min = 5, Max = 10 } },
                        { JewelOptionType.MpRecovery, new CellonGenerator { Min = 8, Max = 15 } }
                    }
                },
                {
                    2, new Dictionary<JewelOptionType, CellonGenerator>
                    {
                        { JewelOptionType.Hp, new CellonGenerator { Min = 120, Max = 200 } },
                        { JewelOptionType.Mp, new CellonGenerator { Min = 150, Max = 250 } },
                        { JewelOptionType.HpRecovery, new CellonGenerator { Min = 14, Max = 20 } },
                        { JewelOptionType.MpRecovery, new CellonGenerator { Min = 16, Max = 25 } }
                    }
                },
                {
                    3, new Dictionary<JewelOptionType, CellonGenerator>
                    {
                        { JewelOptionType.Hp, new CellonGenerator { Min = 220, Max = 330 } },
                        { JewelOptionType.Mp, new CellonGenerator { Min = 280, Max = 330 } },
                        { JewelOptionType.HpRecovery, new CellonGenerator { Min = 22, Max = 28 } },
                        { JewelOptionType.MpRecovery, new CellonGenerator { Min = 16, Max = 25 } }
                    }
                },
                {
                    4, new Dictionary<JewelOptionType, CellonGenerator>
                    {
                        { JewelOptionType.Hp, new CellonGenerator { Min = 330, Max = 400 } },
                        { JewelOptionType.Mp, new CellonGenerator { Min = 350, Max = 420 } },
                        { JewelOptionType.HpRecovery, new CellonGenerator { Min = 30, Max = 38 } },
                        { JewelOptionType.MpRecovery, new CellonGenerator { Min = 36, Max = 45 } }
                    }
                },
                {
                    5, new Dictionary<JewelOptionType, CellonGenerator>
                    {
                        { JewelOptionType.Hp, new CellonGenerator { Min = 430, Max = 550 } },
                        { JewelOptionType.Mp, new CellonGenerator { Min = 550, Max = 550 } },
                        { JewelOptionType.HpRecovery, new CellonGenerator { Min = 40, Max = 50 } },
                        { JewelOptionType.MpRecovery, new CellonGenerator { Min = 50, Max = 60 } }
                    }
                },
                {
                    6, new Dictionary<JewelOptionType, CellonGenerator>
                    {
                        { JewelOptionType.Hp, new CellonGenerator { Min = 600, Max = 750 } },
                        { JewelOptionType.Mp, new CellonGenerator { Min = 600, Max = 750 } },
                        { JewelOptionType.HpRecovery, new CellonGenerator { Min = 55, Max = 70 } },
                        { JewelOptionType.MpRecovery, new CellonGenerator { Min = 65, Max = 80 } },
                        { JewelOptionType.CriticalDamageDecrease, new CellonGenerator { Min = 21, Max = 35 } }
                    }
                },
                {
                    7, new Dictionary<JewelOptionType, CellonGenerator>
                    {
                        { JewelOptionType.Hp, new CellonGenerator { Min = 800, Max = 1000 } },
                        { JewelOptionType.Mp, new CellonGenerator { Min = 800, Max = 1000 } },
                        { JewelOptionType.HpRecovery, new CellonGenerator { Min = 75, Max = 90 } },
                        { JewelOptionType.MpRecovery, new CellonGenerator { Min = 75, Max = 90 } },
                        { JewelOptionType.CriticalDamageDecrease, new CellonGenerator { Min = 11, Max = 20 } }
                    }
                },
                {
                    8, new Dictionary<JewelOptionType, CellonGenerator>
                    {
                        { JewelOptionType.Hp, new CellonGenerator { Min = 1000, Max = 1300 } },
                        { JewelOptionType.Mp, new CellonGenerator { Min = 1000, Max = 1300 } },
                        { JewelOptionType.HpRecovery, new CellonGenerator { Min = 100, Max = 120 } },
                        { JewelOptionType.MpRecovery, new CellonGenerator { Min = 100, Max = 120 } },
                        { JewelOptionType.MpConsumption, new CellonGenerator { Min = 13, Max = 17 } },
                        { JewelOptionType.CriticalDamageDecrease, new CellonGenerator { Min = 21, Max = 35 } }
                    }
                },
                {
                    9, new Dictionary<JewelOptionType, CellonGenerator>
                    {
                        { JewelOptionType.Hp, new CellonGenerator { Min = 1100, Max = 1500 } },
                        { JewelOptionType.Mp, new CellonGenerator { Min = 1100, Max = 1500 } },
                        { JewelOptionType.HpRecovery, new CellonGenerator { Min = 110, Max = 135 } },
                        { JewelOptionType.MpRecovery, new CellonGenerator { Min = 110, Max = 135 } },
                        { JewelOptionType.MpConsumption, new CellonGenerator { Min = 14, Max = 21 } },
                        { JewelOptionType.CriticalDamageDecrease, new CellonGenerator { Min = 22, Max = 45 } }
                    }
                }
            };

        public EquipmentOptionDto GenerateOption(int itemEffectValue)
        {
            Dictionary<JewelOptionType, CellonGenerator> dictionary = _generatorDictionary[itemEffectValue];
            Dictionary<JewelOptionType, CellonGenerator>.ValueCollection list = dictionary.Values;
            var result = new EquipmentOptionDto();
            int rand = new Random().Next(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                if (i != rand)
                {
                    continue;
                }

                result.Value = new Random().Next(list.ElementAt(i).Min, list.ElementAt(i).Max);
                result.Level = (byte)itemEffectValue;
                result.Type = (byte)i;
                return result;
            }

            return null;
        }

        private class CellonGenerator
        {
            public int Min { get; set; }
            public int Max { get; set; }
        }

        #region Singleton

        private static CellonGeneratorHelper _instance;

        public static CellonGeneratorHelper Instance => _instance ?? (_instance = new CellonGeneratorHelper());

        #endregion
    }
}