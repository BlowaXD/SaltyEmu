using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using CommandLine;
using SaltyEmu.BasicAlgorithmPlugin;
using SaltyEmu.Core.Logging;
using SaltyEmu.DatabasePlugin;
using Toolkit.Generators.FromPackets;

namespace Toolkit.Commands
{
    [Verb("generate", HelpText = "Generate Portals, Npcs, Monsters")]
    public class GenerateCommand
    {
        [Value(0, HelpText = "npc, monster, portal, all")]
        public string GeneratorType { get; set; }

        [Option('i', "input", HelpText = "Input directory", Required = true)]
        public string Input { get; set; }

        public static int Handle(GenerateCommand command)
        {
            var algo = new BasicAlgorithmPlugin();
            algo.OnLoad();
            algo.OnEnable();
            var tmp = new DatabasePlugin(Logger.GetLogger<DatabasePlugin>());
            tmp.OnLoad();
            tmp.OnEnable();

            ChickenContainer.Initialize();
            var portal = new PacketPortalGenerator();
            var monster = new MapMonsterGenerator();
            var npc = new MapNpcGenerator();
            var shop = new ShopParserGenerator();
            var shopItem = new ShopItemGenerator();
            switch (command.GeneratorType)
            {
                case "npc":
                    npc.Generate(command.Input);
                    break;
                case "monster":
                    monster.Generate(command.Input);
                    break;
                case "portal":
                    portal.Generate(command.Input);
                    break;
                case "shop":
                    shop.Generate(command.Input);
                    break;
                case "shopItem":
                    shopItem.Generate(command.Input);
                    break;
                case "all":
                    portal.Generate(command.Input);
                    monster.Generate(command.Input);
                    npc.Generate(command.Input);
                    shop.Generate(command.Input);
                    shopItem.Generate(command.Input);
                    break;
            }

            return 0;
        }
    }
}