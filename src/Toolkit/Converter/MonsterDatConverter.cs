using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data;
using ChickenAPI.Data.BCard;
using ChickenAPI.Data.Drop;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.BCard;
using ChickenAPI.Enums.Game.Drop;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Enumerations;

namespace Toolkit.Converter
{
    public class MonsterDatConverter
    {
        private static readonly Logger Log = Logger.GetLogger<MonsterDatConverter>();
        private static readonly string _file = "Monster.dat";
        private static string _inputDirectory;
        private readonly List<BCardDto> _monsterBcards = new List<BCardDto>();
        private readonly List<DropDto> _monsterDrops = new List<DropDto>();


        private readonly List<NpcMonsterDto> _monsters = new List<NpcMonsterDto>();
        private readonly List<NpcMonsterSkillDto> _monsterSkills = new List<NpcMonsterSkillDto>();
        private IBCardService _bcardDb;
        private IDropService _dropsDb;

        private INpcMonsterAlgorithmService _monsterAlgorithm;
        private int _monsterBCardsCount;
        private INpcMonsterService _monsterDb;
        private int _monsterDropsCount;
        private int _monsterSkillsCount;
        private Dictionary<long, SkillDto> _skills;
        private INpcMonsterSkillService _skillsDb;
        private ISkillService _skillService;

        private static void GetVnum(IReadOnlyList<string> currentLine, IMappedDto monster)
        {
            monster.Id = Convert.ToInt16(currentLine[2]);
        }

        private void ParseFile()
        {
            bool itemAreaBegin = false;
            string path = _inputDirectory + '/' + _file;
            string tmp = File.ReadAllText(path, Encoding.GetEncoding(1252));
            string[] lines = tmp.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var monster = new NpcMonsterDto();


            foreach (string line in lines)
            {
                string[] currentLine = line.Split('\t');

                long unknownData = 0;
                if (currentLine.Length > 2 && currentLine[1] == "VNUM")
                {
                    monster = new NpcMonsterDto();
                    itemAreaBegin = true;
                    GetVnum(currentLine, monster);
                }

                if (itemAreaBegin == false)
                {
                    continue;
                }

                if (currentLine.Length > 2 && currentLine[1] == "NAME")
                {
                    GetName(currentLine, monster);
                }
                else if (currentLine.Length > 2 && currentLine[1] == "LEVEL")
                {
                    GetLevel(currentLine, monster);
                }
                else if (currentLine.Length > 3 && currentLine[1] == "RACE")
                {
                    GetRace(currentLine, monster);
                }
                else if (currentLine.Length > 7 && currentLine[1] == "ATTRIB")
                {
                    GetElementsAndResistance(currentLine, monster);
                }
                else if (currentLine.Length > 3 && currentLine[1] == "HP/MP")
                {
                    GetHpMp(currentLine, monster);
                }
                else if (currentLine.Length > 2 && currentLine[1] == "EXP")
                {
                    GetExp(currentLine, monster);
                }

                else if (currentLine.Length > 6 && currentLine[1] == "PREATT")
                {
                    GetPreAtt(currentLine, monster);
                }
                else if (currentLine.Length > 6 && currentLine[1] == "WEAPON")
                {
                    GetWeapon(currentLine, monster);
                }
                else if (currentLine.Length > 6 && currentLine[1] == "ARMOR")
                {
                    GetArmor(currentLine, monster);
                }
                else if (currentLine.Length > 7 && currentLine[1] == "ETC")
                {
                    unknownData = GetEtc(currentLine, monster);
                }
                else if (currentLine.Length > 6 && currentLine[1] == "SETTING")
                {
                    GetSetting(currentLine, monster);
                }
                else if (currentLine.Length > 4 && currentLine[1] == "PETINFO")
                {
                    GetPetInfo(currentLine, monster, unknownData);
                }
                else if (currentLine.Length > 2 && currentLine[1] == "EFF")
                {
                    GetEff(currentLine, monster);
                }
                else if (currentLine.Length > 8 && currentLine[1] == "ZSKILL")
                {
                    GetZskill(currentLine, monster);
                }
                else if (currentLine.Length > 4 && currentLine[1] == "WINFO")
                {
                    GetWinfo(currentLine, monster, unknownData);
                }
                else if (currentLine.Length > 3 && currentLine[1] == "AINFO")
                {
                    GetAInfo(currentLine, monster, unknownData);
                }
                else if (currentLine.Length > 1 && currentLine[1] == "SKILL")
                {
                    GetSkill(currentLine, monster);
                }

                else if (currentLine.Length > 1 && currentLine[1] == "CARD")
                {
                    GetCard(currentLine, monster);
                }
                else if (currentLine.Length > 1 && currentLine[1] == "BASIC")
                {
                    GetBasic(currentLine, monster);
                }
                else if (currentLine.Length > 3 && currentLine[1] == "ITEM")
                {
                    GetItem(currentLine, monster, ref itemAreaBegin);
                }
            }
        }

        private void GetItem(string[] currentLine, NpcMonsterDto monster, ref bool itemAreaBegin)
        {
            for (int i = 2; i < currentLine.Length - 3; i += 3)
            {
                short vnum = Convert.ToInt16(currentLine[i]);
                if (vnum == -1)
                {
                    break;
                }

                _monsterDrops.Add(new DropDto
                {
                    ItemId = vnum,
                    Amount = Convert.ToInt32(currentLine[i + 2]),
                    RelationType = DropType.NpcMonster,
                    TypedId = monster.Id,
                    DropChance = Convert.ToInt32(currentLine[i + 1])
                });
                _monsterDropsCount++;
            }

            _monsters.Add(monster);
            itemAreaBegin = false;
        }

        private void GetBasic(IReadOnlyList<string> currentLine, IMappedDto monster)
        {
            for (int i = 0; i < 4; i++)
            {
                byte type = (byte)int.Parse(currentLine[5 * i + 2]);
                if (type == 0)
                {
                    continue;
                }

                int first = int.Parse(currentLine[5 * i + 5]);

                var monsterCard = new BCardDto
                {
                    RelationType = BCardRelationType.NpcMonster,
                    RelationId = monster.Id,
                    Type = (BCardType)type,
                    SubType = (byte)((int.Parse(currentLine[5 * i + 6]) + 1) * 10 + 1 + (first > 0 ? 0 : 1)),
                    FirstData = (short)((first > 0 ? first : -first) / 4),
                    SecondData = (short)(int.Parse(currentLine[5 * i + 4]) / 4),
                    ThirdData = (short)(int.Parse(currentLine[5 * i + 3]) / 4),
                    CastType = 1,
                    IsLevelScaled = false,
                    IsLevelDivided = false
                };
                _monsterBcards.Add(monsterCard);
                _monsterBCardsCount++;
            }
        }

        private void GetCard(string[] currentLine, NpcMonsterDto monster)
        {
            for (int i = 0; i < 4; i++)
            {
                byte type = (byte)int.Parse(currentLine[5 * i + 2]);
                if (type == 0 || type == 255)
                {
                    continue;
                }

                int first = int.Parse(currentLine[5 * i + 3]);
                var monsterCard = new BCardDto
                {
                    RelationType = BCardRelationType.NpcMonster,
                    RelationId = monster.Id,
                    Type = (BCardType)type,
                    SubType = (byte)(int.Parse(currentLine[5 * i + 5]) + 1 * 10 + 1 + (first > 0 ? 0 : 1)),
                    IsLevelScaled = Convert.ToBoolean(first % 4),
                    IsLevelDivided = (first % 4) == 2,
                    FirstData = (short)((first > 0 ? first : -first) / 4),
                    SecondData = (short)(int.Parse(currentLine[5 * i + 4]) / 4),
                    ThirdData = (short)(int.Parse(currentLine[5 * i + 6]) / 4)
                };
                _monsterBcards.Add(monsterCard);
                _monsterBCardsCount++;
            }
        }

        private void GetSkill(IReadOnlyList<string> currentLine, IMappedDto monster)
        {
            for (int i = 2; i < currentLine.Count - 3; i += 3)
            {
                short vnum = short.Parse(currentLine[i]);
                if (vnum == -1 || vnum == 0)
                {
                    break;
                }

                if (!_skills.ContainsKey(vnum))
                {
                    continue;
                }

                _monsterSkills.Add(new NpcMonsterSkillDto
                {
                    SkillId = vnum,
                    Rate = Convert.ToInt16(currentLine[i + 1]),
                    NpcMonsterId = monster.Id
                });
                _monsterSkillsCount++;
            }
        }

        private static void GetAInfo(IReadOnlyList<string> currentLine, NpcMonsterDto monster, long unknownData)
        {
            monster.DefenceUpgrade = Convert.ToByte(unknownData == 1 ? currentLine[2] : currentLine[3]);
        }

        private static void GetWinfo(IReadOnlyList<string> currentLine, NpcMonsterDto monster, long unknownData)
        {
            monster.AttackUpgrade = Convert.ToByte(unknownData == 1 ? currentLine[2] : currentLine[4]);
        }

        private static void GetZskill(IReadOnlyList<string> currentLine, NpcMonsterDto monster)
        {
            monster.AttackClass = Convert.ToByte(currentLine[2]);
            monster.BasicRange = Convert.ToByte(currentLine[3]);
            monster.BasicArea = Convert.ToByte(currentLine[5]);
            monster.BasicCooldown = Convert.ToInt16(currentLine[6]);
        }

        private static void GetEff(IReadOnlyList<string> currentLine, NpcMonsterDto monster)
        {
            monster.BasicSkill = Convert.ToInt16(currentLine[2]);
        }

        private static void GetPetInfo(IReadOnlyList<string> currentLine, NpcMonsterDto monster, long unknownData)
        {
            if (monster.VNumRequired != 0 || unknownData != -2147481593 && unknownData != -2147481599 && unknownData != -1610610681)
            {
                return;
            }

            monster.VNumRequired = Convert.ToInt16(currentLine[2]);
            monster.AmountRequired = Convert.ToByte(currentLine[3]);
        }

        private static void GetSetting(IReadOnlyList<string> currentLine, NpcMonsterDto monster)
        {
            if (currentLine[4] == "0")
            {
                return;
            }

            monster.VNumRequired = Convert.ToInt16(currentLine[4]);
            monster.AmountRequired = 1;
        }

        private static long GetEtc(IReadOnlyList<string> currentLine, NpcMonsterDto monster)
        {
            int monsterEtcVal = Convert.ToInt32(currentLine[2]);
            monster.CantWalk = Convert.ToBoolean(monsterEtcVal & 0x1);
            monster.CanCollect = Convert.ToBoolean(monsterEtcVal & 0x2);
            monster.CantDebuff = Convert.ToBoolean(monsterEtcVal & 0x4);
            monster.CanCatch = Convert.ToBoolean(monsterEtcVal & 0x8);
            monster.CanRegenMp = Convert.ToBoolean(monsterEtcVal & 0x400);
            monster.CantVoke = Convert.ToBoolean(monsterEtcVal & 0x800);
            monster.CantTargetInfo = Convert.ToBoolean(monsterEtcVal & 0x80000000);

            long unknownData = Convert.ToInt64(currentLine[2]);
            switch (unknownData)
            {
                case -2147481593:
                    monster.MonsterType = MonsterType.Special;
                    break;
                case -2147483616:
                case -2147483647:
                case -2147483646:
                    if (monster.Race == 8 && monster.RaceType == 0)
                    {
                        monster.NoAggresiveIcon = true;
                    }
                    else
                    {
                        monster.NoAggresiveIcon = false;
                    }

                    break;
            }

            if (monster.Id >= 588 && monster.Id <= 607)
            {
                monster.MonsterType = MonsterType.Elite;
            }

            return unknownData;
        }

        private void GetArmor(string[] currentLine, NpcMonsterDto monster)
        {
            monster.CloseDefence = Convert.ToInt16((Convert.ToInt16(currentLine[2]) - 1) * 2 + 18);
            monster.DistanceDefence = Convert.ToInt16((Convert.ToInt16(currentLine[2]) - 1) * 3 + 17);
            monster.MagicDefence = Convert.ToInt16((Convert.ToInt16(currentLine[2]) - 1) * 2 + 13);
            monster.DefenceDodge = Convert.ToInt16((Convert.ToInt16(currentLine[2]) - 1) * 5 + 31);
            monster.DistanceDefenceDodge = Convert.ToInt16((Convert.ToInt16(currentLine[2]) - 1) * 5 + 31);
        }

        private void GetWeapon(string[] currentLine, NpcMonsterDto monster)
        {
            switch (currentLine[3])
            {
                case "1":
                    monster.DamageMinimum = Convert.ToInt16((Convert.ToInt16(currentLine[2]) - 1) * 4 + 32 + Convert.ToInt16(currentLine[4]) +
                        Math.Round(Convert.ToDecimal((monster.Level - 1) / 5)));
                    monster.DamageMaximum = Convert.ToInt16((Convert.ToInt16(currentLine[2]) - 1) * 6 + 40 + Convert.ToInt16(currentLine[5]) -
                        Math.Round(Convert.ToDecimal((monster.Level - 1) / 5)));
                    monster.Concentrate = Convert.ToInt16((Convert.ToInt16(currentLine[2]) - 1) * 5 + 27 + Convert.ToInt16(currentLine[6]));
                    monster.CriticalChance = Convert.ToByte(4 + Convert.ToInt16(currentLine[7]));
                    monster.CriticalRate = Convert.ToInt16(70 + Convert.ToInt16(currentLine[8]));
                    break;
                case "2":
                    monster.DamageMinimum = Convert.ToInt16(Convert.ToInt16(currentLine[2]) * 6.5f + 23 + Convert.ToInt16(currentLine[4]));
                    monster.DamageMaximum = Convert.ToInt16((Convert.ToInt16(currentLine[2]) - 1) * 8 + 38 + Convert.ToInt16(currentLine[5]));
                    monster.Concentrate = Convert.ToInt16(70 + Convert.ToInt16(currentLine[6]));
                    break;
            }
        }

        private void GetPreAtt(IReadOnlyList<string> currentLine, NpcMonsterDto monster)
        {
            monster.IsHostile = currentLine[2] == "1";
            monster.IsHostileGroup = currentLine[3] != "0";
            monster.NoticeRange = Convert.ToByte(currentLine[4]);
            monster.Speed = Convert.ToByte(currentLine[5]);
            monster.RespawnTime = Convert.ToInt32(currentLine[6]);
        }

        private void GetExp(string[] currentLine, NpcMonsterDto monster)
        {
            monster.Xp = _monsterAlgorithm.GetXp(NpcMonsterRaceType.Race0UnknownYet, monster.Level, true) + Math.Abs(Convert.ToInt32(currentLine[2]));
            monster.JobXp = _monsterAlgorithm.GetXp(NpcMonsterRaceType.Race0UnknownYet, monster.Level, true) + Convert.ToInt32(currentLine[3]);
            monster.HeroXp = _monsterAlgorithm.GetHeroXp(NpcMonsterRaceType.Race0UnknownYet, monster.Level, true); // basicHxp


            /*
             * percent damage monsters
             */
            monster.IsPercent = true;
            monster.TakeDamages = 0;
        }

        private void GetHpMp(string[] currentLine, NpcMonsterDto monster)
        {
            monster.MaxHp = _monsterAlgorithm.GetHpMax(NpcMonsterRaceType.Race0UnknownYet, monster.Level, true) + Convert.ToInt32(currentLine[2]);
            monster.MaxMp = _monsterAlgorithm.GetMpMax(NpcMonsterRaceType.Race0UnknownYet, monster.Level, true) + Convert.ToInt32(currentLine[3]);
        }

        private void GetElementsAndResistance(string[] currentLine, NpcMonsterDto monster)
        {
            monster.Element = Enum.Parse<ElementType>(currentLine[2]);
            monster.ElementRate = Convert.ToInt16(currentLine[3]);
            monster.FireResistance = Convert.ToSByte(currentLine[4]);
            monster.WaterResistance = Convert.ToSByte(currentLine[5]);
            monster.LightResistance = Convert.ToSByte(currentLine[6]);
            monster.DarkResistance = Convert.ToSByte(currentLine[7]);
        }

        private static void GetRace(IReadOnlyList<string> currentLine, NpcMonsterDto monster)
        {
            monster.Race = Convert.ToByte(currentLine[2]);
            monster.RaceType = Convert.ToByte(currentLine[3]);
        }

        private void GetLevel(IReadOnlyList<string> currentLine, NpcMonsterDto monster)
        {
            monster.Level = Convert.ToByte(currentLine[2]);
        }

        private void GetName(IReadOnlyList<string> currentLine, NpcMonsterDto monster)
        {
            monster.Name = currentLine[2];
        }

        private void ExtractFiles()
        {
            _monsterDb = ChickenContainer.Instance.Resolve<INpcMonsterService>();
            _bcardDb = ChickenContainer.Instance.Resolve<IBCardService>();
            _skillsDb = ChickenContainer.Instance.Resolve<INpcMonsterSkillService>();
            _dropsDb = ChickenContainer.Instance.Resolve<IDropService>();

            _monsterDb.Save(_monsters);
            _bcardDb.Save(_monsterBcards);
            _dropsDb.Save(_monsterDrops);
            _skillsDb.Save(_monsterSkills);
        }

        public void Extract(string inputDirectory)
        {
            try
            {
                _inputDirectory = inputDirectory;

                _monsterAlgorithm = ChickenContainer.Instance.Resolve<INpcMonsterAlgorithmService>();
                _skillService = ChickenContainer.Instance.Resolve<ISkillService>();
                _skills = _skillService.Get().ToDictionary(dto => dto.Id);
                ParseFile();
                ExtractFiles();
            }
            catch (Exception e)
            {
                Log.Error("Extract", e);
            }
        }
    }
}