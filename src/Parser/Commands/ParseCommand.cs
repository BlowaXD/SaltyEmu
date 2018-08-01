using System;
using System.IO;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using CommandLine;
using NosSharp.BasicAlgorithm;
using NosSharp.DatabasePlugin;
using NosSharp.Parser.Converter;
using NosSharp.Parser.Generators.FromPackets;

namespace NosSharp.Parser.Commands
{
    [Verb("parse")]
    public class ParseCommand
    {
        private static readonly Logger Log = Logger.GetLogger<ParseCommand>();

        [Value(0, Default = "all", HelpText = "Parsing type : card, skill, map, item, einfo, monster")]
        public string ParsingType { get; set; }

        [Option('p', "packet", HelpText = "Packet file")]
        public string PacketFile { get; set; }

        [Option('m', "map", HelpText = "Map files directory")]
        public string MapDirectory { get; set; }

        [Option('i', "input", HelpText = "Input directory", Required = true)]
        public string InputDirectory { get; set; }

        public static bool CheckFiles(string inputDirectory)
        {
            if (!File.Exists(inputDirectory + "/dats/Skill.dat"))
            {
                return true;
            }
            if (!File.Exists(inputDirectory + "/dats/Monster.dat"))
            {
                return true;
            }
            if (!File.Exists(inputDirectory + "/dats/Item.dat"))
            {
                return true;
            }
            if (!File.Exists(inputDirectory + "/dats/Card.dat"))
            {
                return true;
            }
            if (!File.Exists(inputDirectory + "/dats/Card.dat"))
            {
                return true;
            }
            if (!File.Exists(inputDirectory + "/dats/Card.dat"))
            {
                return true;
            }

            if (!File.Exists(inputDirectory + "/packets/einfo.packet"))
            {
                return true;
            }
            if (!File.Exists(inputDirectory + "/packets/portals.packet"))
            {
                return true;
            }
            if (!File.Exists(inputDirectory + "/packets/monster.packet"))
            {
                return true;
            }
            if (!File.Exists(inputDirectory + "/packets/packet.txt"))
            {
                return true;
            }
            return false;
        }

        public static int Handle(ParseCommand command)
        {
            if (CheckFiles(command.InputDirectory))
            {
                Log.Warn("Respect the following parsing directory layer : ");
                Console.WriteLine($"{command.InputDirectory}/");
                Console.WriteLine($"\t- maps");
                Console.WriteLine($"\t- dats");
                Console.WriteLine($"\t\t- Skill.dat");
                Console.WriteLine($"\t\t- Monster.dat");
                Console.WriteLine($"\t\t- Item.dat");
                Console.WriteLine($"\t\t- Card.dat");
                Console.WriteLine($"\t- packets");
                Console.WriteLine($"\t\t- einfo.packet");
                Console.WriteLine($"\t\t- portals.packet");
                Console.WriteLine($"\t\t- monster.packet");
                Console.WriteLine($"\t\t- packet.txt");
                return 1;
            }

            var algo = new BasicAlgorithmPlugin();
            algo.OnLoad();
            algo.OnEnable();
            var tmp = new NosSharpDatabasePlugin();
            tmp.OnLoad();
            tmp.OnEnable();
            Container.Initialize();

            var card = new CardDatConverter();
            var item = new ItemDatConverter();
            var monster = new MonsterDatConverter();
            var map = new MapDatConverter();
            var skill = new SkillDatConverter();
            var einfo = new EInfoFiller();
            var portal = new PacketPortalGenerator();
            var monGenerator = new MapMonsterGenerator();
            var npc = new MapNpcGenerator();
            var shop = new ShopParserGenerator();
            var shopItem = new ShopItemGenerator();
            switch (command.ParsingType)
            {
                case "card":
                    card.Extract(command.InputDirectory);
                    break;
                case "skill":
                    skill.Extract(command.InputDirectory);
                    break;
                case "map":
                    map.Extract(command.InputDirectory);
                    break;
                case "item":
                    item.Extract(command.InputDirectory);
                    break;
                case "einfo":
                    einfo.Fill(command.PacketFile);
                    break;
                case "monster":
                    monster.Extract(command.InputDirectory);
                    break;
                case "all":
                    if (string.IsNullOrEmpty(command.PacketFile))
                    {
                        return 1;
                    }

                    Log.Info("Parsing...");
                    map.Extract(command.InputDirectory + "/maps");
                    skill.Extract(command.InputDirectory + "/dats");
                    item.Extract(command.InputDirectory + "/dats");
                    card.Extract(command.InputDirectory + "/dats");
                    monster.Extract(command.InputDirectory + "/dats");
                    einfo.Fill(command.InputDirectory + "/packets/einfo.packet");
                    portal.Generate(command.InputDirectory + "/packets/portals.packet");
                    monGenerator.Generate(command.InputDirectory + "/packets/monster.packet");
                    npc.Generate(command.InputDirectory + "/packets/monster.packet");
                    shop.Generate(command.InputDirectory + "/packets/packet.txt");
                    shopItem.Generate(command.InputDirectory + "/packets/packet.txt");
                    Log.Info("Parsing done");
                    break;
            }

            return 0;
        }
    }
}