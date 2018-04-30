using System;
using System.IO;
using System.Net;
using ChickenAPI.Accounts;
using ChickenAPI.DAL.Interfaces;
using ChickenAPI.Dtos;
using ChickenAPI.Packets;
using ChickenAPI.Plugin;
using ChickenAPI.Utils;
using NosSharp.World.Cryptography;
using NosSharp.World.Extensions;
using NosSharp.World.Network;
using NosSharp.World.Packets;
using NosSharp.World.Session;
using NosSharp.World.Utils;

namespace NosSharp.World
{
    internal class WorldServer
    {
        private static string _ip;
        private static int _port;

        private static void InitializePlugins()
        {
            DependencyContainer.Instance.Register<IPluginManager>(new SimplePluginManager());
            IPlugin[] plugins = DependencyContainer.Instance.Get<IPluginManager>().LoadPlugins(new DirectoryInfo("plugins"));
            if (plugins == null)
            {
                return;
            }

            foreach (IPlugin plugin in plugins)
            {
                plugin.OnEnable();
            }
        }

        private static void InitializeConfigs()
        {
            string port = Environment.GetEnvironmentVariable("SERVER_PORT");
            if (!int.TryParse(port, out _port))
            {
                _port = 1337;
            }

            _ip = Environment.GetEnvironmentVariable("SERVER_IP");
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
            DependencyContainer.Instance.Register<IPacketFactory>(new PluggablePacketFactory());
            DependencyContainer.Instance.Register<IPacketHandler>(new PacketHandler());
            InitializePlugins();
            ClientSession.SetPacketFactory(DependencyContainer.Instance.Get<IPacketFactory>());
            ClientSession.SetPacketHandler(DependencyContainer.Instance.Get<IPacketHandler>());
            Console.WriteLine($"\n\nListening on port {_port}");
            Server.RunServerAsync(_port, new WorldCryptoFactory()).Wait();
        }
    }
}