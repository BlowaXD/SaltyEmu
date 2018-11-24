using System;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;

namespace SaltyEmu.RelationService
{
    internal class Program
    {
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
            InitializeAsync().Wait();
        }

        private static Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}