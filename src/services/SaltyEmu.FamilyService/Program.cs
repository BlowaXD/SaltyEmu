using System;
using System.Linq;
using ChickenAPI.Core.Logging;

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
        }

        private static void Main()
        {
            PrintHeader();
            InitializeLogger();
        }
    }
}