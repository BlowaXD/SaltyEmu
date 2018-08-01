using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.AccessLayer.BCard;
using ChickenAPI.Data.AccessLayer.Skill;
using ChickenAPI.Data.TransferObjects.BCard;
using ChickenAPI.Data.TransferObjects.Skills;
using ChickenAPI.Enums.Game.BCard;

namespace Toolkit.Converter
{
    public class SkillDatConverter
    {
        private const string FILE = "Skill.dat";
        private static readonly Logger Log = Logger.GetLogger<CardDatConverter>();
        private static string _inputDirectory;

        private readonly Dictionary<long, SkillDto> _skills = new Dictionary<long, SkillDto>();
        private readonly List<BCardDto> _skillBCards = new List<BCardDto>();
        private int _skillCount;
        private int _skillBCardsCount;

        private ISkillService _skillService;
        private IBCardService _bcardService;

        private void ParseFile()
        {
            bool itemAreaBegin = false;
            string path = _inputDirectory + '/' + FILE;
            string tmp = File.ReadAllText(path, Encoding.GetEncoding(1252));
            string[] lines = tmp.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var skill = new SkillDto();


            foreach (string line in lines)
            {
                string[] currentLine = line.Split('\t');
                if (currentLine.Length > 2 && currentLine[1] == "VNUM")
                {
                    skill = new SkillDto
                    {
                        Id = short.Parse(currentLine[2])
                    };
                    itemAreaBegin = true;
                }

                if (itemAreaBegin == false)
                {
                    continue;
                }

                if (currentLine.Length > 2 && currentLine[1] == "NAME")
                {
                    skill.Name = currentLine[2];
                }
                else if (currentLine.Length > 2 && currentLine[1] == "TYPE")
                {
                    GetTypeData(currentLine, skill);
                }
                else if (currentLine.Length > 2 && currentLine[1] == "FCOMBO")
                {
                    GetFCombo(currentLine, skill);
                }
                else if (currentLine.Length > 3 && currentLine[1] == "COST")
                {
                    skill.CpCost = currentLine[2] == "-1" ? (byte)0 : byte.Parse(currentLine[2]);
                    skill.Price = int.Parse(currentLine[3]);
                }
                else if (currentLine.Length > 2 && currentLine[1] == "LEVEL")
                {
                    GetLevel(currentLine, skill);
                }
                else if (currentLine.Length > 2 && currentLine[1] == "EFFECT")
                {
                    GetEffect(currentLine, skill);
                }
                else if (currentLine.Length > 2 && currentLine[1] == "TARGET")
                {
                    GetTarget(currentLine, skill);
                }
                else if (currentLine.Length > 2 && currentLine[1] == "DATA")
                {
                    GetData(currentLine, skill);
                }
                else if (currentLine.Length > 2 && currentLine[1] == "BASIC")
                {
                    GetBasic(currentLine, skill);
                }
                else if (currentLine.Length > 2 && currentLine[1] == "CELL")
                {
                    // investigate
                }
                else if (currentLine.Length > 1 && currentLine[1] == "Z_DESC")
                {
                    // investigate

                    _skills.Add(skill.Id, skill);
                    _skillCount++;
                }
            }
        }

        private void GetEffect(string[] currentLine, SkillDto skill)
        {
            skill.CastEffect = short.Parse(currentLine[3]);
            skill.CastAnimation = short.Parse(currentLine[4]);
            skill.Effect = short.Parse(currentLine[5]);
            skill.AttackAnimation = short.Parse(currentLine[6]);
        }

        private void GetTarget(IReadOnlyList<string> currentLine, SkillDto skill)
        {
            skill.TargetType = byte.Parse(currentLine[2]);
            skill.HitType = byte.Parse(currentLine[3]);
            skill.Range = byte.Parse(currentLine[4]);
            skill.TargetRange = byte.Parse(currentLine[5]);
        }

        private static void GetTypeData(IReadOnlyList<string> currentLine, SkillDto skill)
        {
            skill.SkillType = byte.Parse(currentLine[2]);
            skill.CastId = short.Parse(currentLine[3]);
            skill.Class = byte.Parse(currentLine[4]);
            skill.Type = byte.Parse(currentLine[5]);
            skill.Element = byte.Parse(currentLine[7]);
        }

        private void GetFCombo(string[] currentLine, SkillDto skill)
        {
            for (int i = 3; i < currentLine.Length - 4; i += 3)
            {
                /*
                var comb = new ComboDTO
                {
                    SkillVNum = skill.SkillVNum,
                    Hit = short.Parse(currentLine[i]),
                    Animation = short.Parse(currentLine[i + 1]),
                    Effect = short.Parse(currentLine[i + 2])
                };

                if (comb.Hit == 0 && comb.Animation == 0 && comb.Effect == 0)
                {
                    continue;
                }

                if (!DaoFactory.ComboDao.LoadByVNumHitAndEffect(comb.SkillVNum, comb.Hit, comb.Effect).Any())
                {
                    combo.Add(comb);
                }
                */
            }
        }

        private void GetLevel(IReadOnlyList<string> currentLine, SkillDto skill)
        {
            skill.LevelMinimum = currentLine[2] != "-1" ? byte.Parse(currentLine[2]) : (byte)0;
            if (skill.Class > 31)
            {
                SkillDto firstskill = _skills.FirstOrDefault(s => s.Value.Class == skill.Class).Value;
                if (firstskill == null || skill.Id <= firstskill.Id + 10)
                {
                    switch (skill.Class)
                    {
                        case 8:
                            switch (_skills.Count(s => s.Value.Class == skill.Class))
                            {
                                case 3:
                                    skill.LevelMinimum = 20;
                                    break;

                                case 2:
                                    skill.LevelMinimum = 10;
                                    break;

                                default:
                                    skill.LevelMinimum = 0;
                                    break;
                            }

                            break;

                        case 9:
                            switch (_skills.Count(s => s.Value.Class == skill.Class))
                            {
                                case 9:
                                    skill.LevelMinimum = 20;
                                    break;

                                case 8:
                                    skill.LevelMinimum = 16;
                                    break;

                                case 7:
                                    skill.LevelMinimum = 12;
                                    break;

                                case 6:
                                    skill.LevelMinimum = 8;
                                    break;

                                case 5:
                                    skill.LevelMinimum = 4;
                                    break;

                                default:
                                    skill.LevelMinimum = 0;
                                    break;
                            }

                            break;

                        case 16:
                            switch (_skills.Count(s => s.Value.Class == skill.Class))
                            {
                                case 6:
                                    skill.LevelMinimum = 20;
                                    break;

                                case 5:
                                    skill.LevelMinimum = 15;
                                    break;

                                case 4:
                                    skill.LevelMinimum = 10;
                                    break;

                                case 3:
                                    skill.LevelMinimum = 5;
                                    break;

                                case 2:
                                    skill.LevelMinimum = 3;
                                    break;

                                default:
                                    skill.LevelMinimum = 0;
                                    break;
                            }

                            break;

                        default:
                            switch (_skills.Count(s => s.Value.Class == skill.Class))
                            {
                                case 10:
                                    skill.LevelMinimum = 20;
                                    break;

                                case 9:
                                    skill.LevelMinimum = 16;
                                    break;

                                case 8:
                                    skill.LevelMinimum = 12;
                                    break;

                                case 7:
                                    skill.LevelMinimum = 8;
                                    break;

                                case 6:
                                    skill.LevelMinimum = 4;
                                    break;

                                default:
                                    skill.LevelMinimum = 0;
                                    break;
                            }

                            break;
                    }
                }
            }

            skill.MinimumAdventurerLevel = currentLine[3] != "-1" ? byte.Parse(currentLine[3]) : (byte)0;
            skill.MinimumSwordmanLevel = currentLine[4] != "-1" ? byte.Parse(currentLine[4]) : (byte)0;
            skill.MinimumArcherLevel = currentLine[5] != "-1" ? byte.Parse(currentLine[5]) : (byte)0;
            skill.MinimumMagicianLevel = currentLine[6] != "-1" ? byte.Parse(currentLine[6]) : (byte)0;
        }

        private static void GetData(IReadOnlyList<string> currentLine, SkillDto skill)
        {
            skill.UpgradeSkill = short.Parse(currentLine[2]);
            skill.UpgradeType = short.Parse(currentLine[3]);
            skill.CastTime = short.Parse(currentLine[6]);
            skill.Cooldown = short.Parse(currentLine[7]);
            skill.MpCost = short.Parse(currentLine[10]);
            skill.ItemVNum = short.Parse(currentLine[12]);
        }

        private void GetBasic(IReadOnlyList<string> currentLine, IMappedDto skill)
        {
            byte type = (byte)int.Parse(currentLine[3]);
            if (type == 0 || type == 255)
            {
                return;
            }

            int first = int.Parse(currentLine[5]);
            var itemCard = new BCardDto
            {
                RelationType = BCardRelationType.Skill,
                RelationId = skill.Id,
                Type = type,
                SubType = (byte)((int.Parse(currentLine[4]) + 1) * 10 + 1 + (first < 0 ? 1 : 0)),
                IsLevelScaled = Convert.ToBoolean(first % 4),
                IsLevelDivided = (first % 4) == 2,
                FirstData = (short)((first > 0 ? first : -first) / 4),
                SecondData = (short)(int.Parse(currentLine[6]) / 4),
                ThirdData = (short)(int.Parse(currentLine[7]) / 4)
            };
            _skillBCards.Add(itemCard);
            _skillBCardsCount++;
        }

        private void ExtractFiles()
        {
            _skillService = Container.Instance.Resolve<ISkillService>();
            _bcardService = Container.Instance.Resolve<IBCardService>();

            _skillService.Save(_skills.Values);
            _bcardService.Save(_skillBCards);
        }

        public void Extract(string inputDirectory)
        {
            try
            {
                _inputDirectory = inputDirectory;

                ParseFile();
                ExtractFiles();
            }
            catch (Exception e)
            {
                Log.Error("Extract", e);
                throw;
            }
        }
    }
}