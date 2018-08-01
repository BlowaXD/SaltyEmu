using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using CommandLine;
using NosSharp.BasicAlgorithm;
using NosSharp.DatabasePlugin;
using NosSharp.Parser.Converter;

namespace NosSharp.Parser.Commands
{
    [Verb("parse")]
    public class ParseCommand
    {
        private static readonly Logger Log = Logger.GetLogger<ParseCommand>();

        [Value(0, Default = "all", HelpText = "Parsing type : card, skill, map, item, einfo, monster")]
        public string ParsingType { get; set; }

        [Option("db", Default = false, HelpText = "Parsing uses database plugin")]
        public bool IsDb { get; set; }

        [Option("fs", Default = false, HelpText = "Parsing uses filesystem plugin")]
        public bool IsFs { get; set; }

        [Option('v', "verbose", Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('p', "packet", HelpText = "Packet file")]
        public string PacketFile { get; set; }

        [Option('m', "map", HelpText = "Map files directory")]
        public string MapDirectory { get; set; }

        [Option('i', "input", HelpText = "Input directory", Required = true)]
        public string InputDirectory { get; set; }

        public static int Handle(ParseCommand command)
        {
            var algo = new BasicAlgorithmPlugin();
            algo.OnLoad();
            algo.OnEnable();
            if (command.IsDb)
            {
                var tmp = new NosSharpDatabasePlugin();
                tmp.OnLoad();
                tmp.OnEnable();
            }

            if (command.Verbose)
            {
                Logger.Initialize();
            }

            Container.Initialize();

            var card = new CardDatConverter();
            var item = new ItemDatConverter();
            var monster = new MonsterDatConverter();
            var map = new MapDatConverter();
            var skill = new SkillDatConverter();
            var einfo = new EInfoFiller();
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
                    einfo.Fill(command.PacketFile);
                    Log.Info("Parsing done");
                    break;
            }

            return 0;
        }
    }
}