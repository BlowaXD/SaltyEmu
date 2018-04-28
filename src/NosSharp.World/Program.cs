using System;
using System.IO;
using System.Net;
using ChickenAPI.Plugin;
using ChickenAPI.Utils;
using NosSharp.World.Cryptography;
using NosSharp.World.Network;
using NosSharp.World.Utils;

namespace NosSharp.World
{
    internal class Program
    {
        private static string _ip;
        private static int _port;

        private static void InitializePlugins()
        {
            DependencyContainer.Instance.Get<IPluginManager>().LoadPlugins(new DirectoryInfo("plugins"));
        }

        private static void InitializeConfigs()
        {
            string port = Environment.GetEnvironmentVariable("SERVER_PORT");
            if (!int.TryParse(port, out _port))
            {
                _port = 1337;
            }
            _ip = Environment.GetEnvironmentVariable("SERVER_IP");
            DependencyContainer.Instance.Register<IPluginManager>(new SimplePluginManager());
        }

        private static void PrintHeader()
        {
            Console.Title = "Nos# - WORLD";
            const string text = "WORLD SERVER - Nos#";
            int offset = Console.WindowWidth / 2 + text.Length / 2;
            string separator = new string('=', Console.WindowWidth);
            Console.WriteLine(separator + string.Format("{0," + offset + "}\n", text) + separator);
        }

        static void Main()
        {
            PrintHeader();
            InitializeConfigs();
            InitializePlugins();
            Server.RunServerAsync(_port, new WorldCryptoFactory()).Wait();
        }
    }
}