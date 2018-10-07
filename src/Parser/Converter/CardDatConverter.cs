using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.BCard;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.BCard;
using ChickenAPI.Enums.Game.Buffs;
using ChickenAPI.Game.Data.AccessLayer.BCard;
using ChickenAPI.Game.Data.AccessLayer.Skill;

namespace Toolkit.Converter
{
    public class CardDatConverter
    {
        private const string FILE = "Card.dat";
        private static readonly Logger Log = Logger.GetLogger<CardDatConverter>();
        private static string _inputDirectory;
        private readonly Queue<BCardDto> _cardBcards = new Queue<BCardDto>();

        private readonly Queue<CardDto> _cards = new Queue<CardDto>();
        private IBCardService _bcardDb;
        private int _cardBCardsCount;
        private int _cardCount;

        private ICardService _cardDb;

        private static void GetVnum(string[] currentLine, CardDto card)
        {
            card.Id = Convert.ToInt16(currentLine[2]);
        }

        private void ParseFile()
        {
            bool itemAreaBegin = false;
            string path = _inputDirectory + '/' + FILE;
            string tmp = File.ReadAllText(path, Encoding.GetEncoding(1252));
            string[] lines = tmp.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var card = new CardDto();


            foreach (string line in lines)
            {
                string[] currentLine = line.Split('\t');
                if (currentLine.Length > 2 && currentLine[1] == "VNUM")
                {
                    card = new CardDto();
                    itemAreaBegin = true;
                    GetVnum(currentLine, card);
                }

                if (itemAreaBegin == false)
                {
                    continue;
                }

                if (currentLine.Length > 2 && currentLine[1] == "NAME")
                {
                    GetName(currentLine, card);
                }
                else if (currentLine.Length > 3 && currentLine[1] == "GROUP")
                {
                    GetLevel(currentLine, card);
                }
                else if (currentLine.Length > 3 && currentLine[1] == "EFFECT")
                {
                    GetEffect(currentLine, card);
                }
                else if (currentLine.Length > 3 && currentLine[1] == "STYLE")
                {
                    GetBuff(currentLine, card);
                }
                else if (currentLine.Length > 3 && currentLine[1] == "TIME")
                {
                    GetTime(currentLine, card);
                }
                else if (currentLine.Length > 3 && currentLine[1] == "1ST")
                {
                    GetBCards(currentLine, card, 3);
                }
                else if (currentLine.Length > 3 && currentLine[1] == "2ST")
                {
                    GetBCards(currentLine, card, 2);
                }
                else if (currentLine.Length > 3 && currentLine[1] == "LAST")
                {
                    GetTimeout(currentLine, card);

                    _cards.Enqueue(card);
                    _cardCount++;
                    itemAreaBegin = false;
                }
            }
        }

        private void GetBCards(string[] currentLine, CardDto card, byte max)
        {
            for (int i = 0; i < max; i++)
            {
                if (currentLine[2 + i * 6] == "-1" || currentLine[2 + i * 6] == "0")
                {
                    continue;
                }

                int first = int.Parse(currentLine[i * 6 + 6]);
                var bcard = new BCardDto
                {
                    RelationType = BCardRelationType.Card,
                    RelationId = card.Id,
                    Type = byte.Parse(currentLine[2 + i * 6]),
                    SubType = (byte)((Convert.ToByte(currentLine[3 + i * 6]) + 1) * 10 + 1 + (first < 0 ? 1 : 0)),
                    FirstData = (first > 0 ? first : -first) / 4,
                    SecondData = int.Parse(currentLine[7 + i * 6]) / 4,
                    ThirdData = int.Parse(currentLine[5 + i * 6]),
                    IsLevelScaled = Convert.ToBoolean(first % 4),
                    IsLevelDivided = (first % 4) == 2
                };
                _cardBCardsCount++;
                _cardBcards.Enqueue(bcard);
            }
        }

        private void GetTimeout(string[] currentLine, CardDto card)
        {
            card.TimeoutBuff = short.Parse(currentLine[2]);
            card.TimeoutBuffChance = byte.Parse(currentLine[3]);
        }

        private void GetTime(string[] currentLine, CardDto card)
        {
            card.Duration = Convert.ToInt32(currentLine[2]);
            card.Delay = Convert.ToInt32(currentLine[3]);
        }

        private void GetBuff(string[] currentLine, CardDto card)
        {
            card.BuffType = (BuffType)Convert.ToByte(currentLine[3]);
        }

        private static void GetEffect(IReadOnlyList<string> currentLine, CardDto card)
        {
            card.EffectId = Convert.ToInt32(currentLine[2]);
        }

        private void GetLevel(IReadOnlyList<string> currentLine, CardDto card)
        {
            card.Level = Convert.ToByte(currentLine[3]);
        }

        private void GetName(IReadOnlyList<string> currentLine, CardDto card)
        {
            card.Name = currentLine[2];
        }

        private void ExtractFiles()
        {
            _cardDb = ChickenContainer.Instance.Resolve<ICardService>();
            _bcardDb = ChickenContainer.Instance.Resolve<IBCardService>();
            _cardDb.Save(_cards);
            _bcardDb.Save(_cardBcards);
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