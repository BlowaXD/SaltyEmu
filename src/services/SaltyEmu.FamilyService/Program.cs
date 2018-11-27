using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Families;
using MessagePack.Resolvers;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FamilyPlugin;
using SaltyEmu.FamilyPlugin.Communication;
using SaltyEmu.FamilyService.Handlers;

namespace SaltyEmu.FamilyService
{
    internal class Program
    {
        private static readonly Logger Logger = Logger.GetLogger<Program>();

        private static void PrintHeader()
        {
            Console.Title = "SaltyEmu - Family";
            const string text = @"
███████╗ █████╗ ██╗  ████████╗██╗   ██╗███████╗███╗   ███╗██╗   ██╗   ███████╗ █████╗ ███╗   ███╗██╗██╗  ██╗   ██╗
██╔════╝██╔══██╗██║  ╚══██╔══╝╚██╗ ██╔╝██╔════╝████╗ ████║██║   ██║   ██╔════╝██╔══██╗████╗ ████║██║██║  ╚██╗ ██╔╝
███████╗███████║██║     ██║    ╚████╔╝ █████╗  ██╔████╔██║██║   ██║   █████╗  ███████║██╔████╔██║██║██║   ╚████╔╝ 
╚════██║██╔══██║██║     ██║     ╚██╔╝  ██╔══╝  ██║╚██╔╝██║██║   ██║   ██╔══╝  ██╔══██║██║╚██╔╝██║██║██║    ╚██╔╝  
███████║██║  ██║███████╗██║      ██║   ███████╗██║ ╚═╝ ██║╚██████╔╝██╗██║     ██║  ██║██║ ╚═╝ ██║██║███████╗██║   
╚══════╝╚═╝  ╚═╝╚══════╝╚═╝      ╚═╝   ╚══════╝╚═╝     ╚═╝ ╚═════╝ ╚═╝╚═╝     ╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚══════╝╚═╝   
";
            string separator = new string('=', Console.WindowWidth);
            string logo = text.Split('\n').Select(s => string.Format("{0," + (Console.WindowWidth / 2 + s.Length / 2) + "}\n", s))
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
            InitializeClient().ConfigureAwait(false).GetAwaiter().GetResult();


            Console.ReadLine();
        }

        private static async Task InitializeClient()
        {
            MqttClientConfigurationBuilder builder = new MqttClientConfigurationBuilder()
                .ConnectTo("localhost")
                .WithName("family-client-test")
                .WithRequestTopic("/family/request")
                .WithResponseTopic("/family/response")
                .WithSerializer(new JsonSerializer());

            var client = new FamilyIpcClient(builder);
            if (client is MappedRepositoryMqtt<FamilyDto> server)
            {
                await server.InitializeAsync();
            }

            Logger.Info("Asking to get family : \"test\"");
            FamilyDto test = await client.GetByNameAsync("test");
            Logger.Info($"{test?.Name} ID : {test?.Id}");


            Logger.Info("Asking to get family : \"real\"");
            FamilyDto real = await client.GetByNameAsync("real");
            Logger.Info($"{real.Name} ID : {real.Id}");
        }

        private static async Task InitializeAsync()
        {
            var handler = new RequestHandler();
            handler.Register<GetFamilyInformationRequest>(FamilyGetInformationRequestHandler.OnMessage);

            MqttServerConfigurationBuilder builder = new MqttServerConfigurationBuilder()
                .ConnectTo("localhost")
                .WithName("family-server")
                .AddTopic("/family/request")
                .WithResponseTopic("/family/response")
                .WithSerializer(new JsonSerializer())
                .WithRequestHandler(handler);

            var tmp = new FamilyServer(builder);
            if (tmp is MqttIpcServer<FamilyServer> server)
            {
                await server.InitializeAsync();
            }
        }
    }
}