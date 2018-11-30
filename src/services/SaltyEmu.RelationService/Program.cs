using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Families;
using NLog.Fluent;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;
using SaltyEmu.FamilyPlugin;
using SaltyEmu.RelationService.String;

namespace SaltyEmu.RelationService
{
    internal class Program
    {
        private static int _ids = 1;
        private static void PrintHeader()
        {
            Console.Title = "SaltyEmu - Family";
            const string text = @"
███████╗ █████╗ ██╗  ████████╗██╗   ██╗███████╗███╗   ███╗██╗   ██╗    ███████╗██████╗ ██╗███████╗███╗   ██╗██████╗ ███████╗
██╔════╝██╔══██╗██║  ╚══██╔══╝╚██╗ ██╔╝██╔════╝████╗ ████║██║   ██║    ██╔════╝██╔══██╗██║██╔════╝████╗  ██║██╔══██╗██╔════╝
███████╗███████║██║     ██║    ╚████╔╝ █████╗  ██╔████╔██║██║   ██║    █████╗  ██████╔╝██║█████╗  ██╔██╗ ██║██║  ██║███████╗
╚════██║██╔══██║██║     ██║     ╚██╔╝  ██╔══╝  ██║╚██╔╝██║██║   ██║    ██╔══╝  ██╔══██╗██║██╔══╝  ██║╚██╗██║██║  ██║╚════██║
███████║██║  ██║███████╗██║      ██║   ███████╗██║ ╚═╝ ██║╚██████╔╝    ██║     ██║  ██║██║███████╗██║ ╚████║██████╔╝███████║
╚══════╝╚═╝  ╚═╝╚══════╝╚═╝      ╚═╝   ╚══════╝╚═╝     ╚═╝ ╚═════╝     ╚═╝     ╚═╝  ╚═╝╚═╝╚══════╝╚═╝  ╚═══╝╚═════╝ ╚══════╝
";
            Console.WindowWidth = text.Split('\n')[1].Length + (((text.Split('\n')[1].Length % 2) == 0) ? 1 : 2);
            string separator = new string('=', Console.WindowWidth);
            string logo = text.Split('\n').Select(s => string.Format("{0," + (Console.WindowWidth / 2 + separator.Length / 2) + "}\n", s))
                .Aggregate("", (current, i) => current + i);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(separator + logo + separator);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void InitializeLogger()
        {
            Logger.Initialize();
        }

        private static void Main()
        {
            PrintHeader();
            InitializeLogger();
            InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Logger log = Logger.GetLogger<Program>();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            while (true)
            {
                string yoyo = "".RandomString(10);
                log.Info($"Asking to save family : \"{yoyo}\"");
                FamilyDto test = client.Save(new FamilyDto
                {
                    Id = ++_ids,
                    Name = yoyo
                });

                if (test == null)
                {
                    Log.Warn($"{yoyo} could not be saved");
                    break;
                }

                log.Info($"{test?.Name} ID : {test?.Id}");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            Console.ReadLine();
        }

        private static FamilyIpcClient client;

        private static async Task InitializeAsync()
        {
            MqttClientConfigurationBuilder builder = new MqttClientConfigurationBuilder()
                .ConnectTo("localhost")
                .WithName("family-client-test")
                .WithRequestTopic("/family/request")
                .WithBroadcastTopic("/family/broadcast")
                .WithResponseTopic("/family/response")
                .WithSerializer(new JsonSerializer());

            client = new FamilyIpcClient(builder);
            if (client is MappedRepositoryMqtt<FamilyDto> server)
            {
                await server.InitializeAsync();
            }
        }
    }
}