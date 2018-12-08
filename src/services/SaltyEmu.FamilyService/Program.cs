using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Families;
using MessagePack.Resolvers;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Protocol.RepositoryPacket;
using SaltyEmu.Communication.Serializers;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FamilyPlugin;
using SaltyEmu.FamilyPlugin.Communication;
using SaltyEmu.FamilyService.FamilyService;
using SaltyEmu.FamilyService.Handlers;
using SaltyEmu.Redis;
using SaltyEmu.RedisWrappers;

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
            Console.WindowWidth = Console.WindowWidth < text.Split('\n')[1].Length ? text.Split('\n')[1].Length : Console.WindowWidth;
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
            Console.ReadLine();
        }

        private static async Task InitializeAsync()
        {
            var handler = new RequestHandler();

            AsyncRpcRepository<FamilyDto, long> test = new AsyncRpcRepository<FamilyDto, long>(
                new FamilyDao(new RedisConfiguration
                {
                    Host = "localhost",
                    Port = 6379
                }), handler);


            MqttServerConfigurationBuilder builder = new MqttServerConfigurationBuilder()
                .ConnectTo("localhost")
                .WithName("family-server")
                .AddTopic("/family/request")
                .WithBroadcastTopic("/family/broadcast")
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