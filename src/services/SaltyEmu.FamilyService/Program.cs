using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FamilyPlugin;

namespace SaltyEmu.FamilyService
{
    internal class Program
    {
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
            Console.ReadLine();
        }

        private static async Task InitializeAsync()
        {
            MqttServerConfigurationBuilder builder = new MqttServerConfigurationBuilder()
                .ConnectTo("localhost")
                .WithName("family-server")
                .AddTopic("/family/request")
                .WithResponseTopic("/family/response")
                .WithSerializer(new JsonSerializer())
                .WithRequestHandler(new RequestHandler());

            var tmp = new FamilyServer(builder);
            if (tmp is MqttIpcServer<FamilyServer> server)
            {
                await server.InitializeAsync();
            }
        }
    }
}