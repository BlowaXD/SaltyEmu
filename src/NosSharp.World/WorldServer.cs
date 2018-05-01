using System;
using System.IO;
using ChickenAPI.Accounts;
using ChickenAPI.DAL.Interfaces;
using ChickenAPI.Dtos;
using ChickenAPI.Enums;
using ChickenAPI.Packets;
using ChickenAPI.Plugin;
using ChickenAPI.Utils;
using Newtonsoft.Json;
using NosSharp.World.Cryptography;
using NosSharp.World.Network;
using NosSharp.World.Packets;
using NosSharp.World.Session;
using NosSharp.World.Utils;

namespace NosSharp.World
{
    internal class WorldServer
    {
        private static string _worldGroup;
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

            _ip = Environment.GetEnvironmentVariable("SERVER_OUTPUT_IP") ?? "127.0.0.1";
            _worldGroup = Environment.GetEnvironmentVariable("SERVER_OUTPUT_WORLDGROUP") ?? "NosWings";
        }

        private static void PrintHeader()
        {
            Console.Title = "Nos# - WORLD";
            const string text = "WORLD SERVER - Nos#";
            int offset = Console.WindowWidth / 2 + text.Length / 2;
            string separator = new string('=', Console.WindowWidth);
            Console.WriteLine(separator + string.Format("{0," + offset + "}\n", text) + separator);
        }

        private static bool RegisterServer()
        {
            var worldServer = new WorldServerDto
            {
                WorldGroup = _worldGroup,
                Ip = _ip,
                Port = _port,
                Color = ChannelColor.White,
                Id = Guid.Empty,
                ChannelId = 0
            };
            var api = DependencyContainer.Instance.Get<IServerApiService>();
            if (api.RegisterServer(worldServer))
            {
                return true;
            }
            Server.WorldServer = worldServer;
            return false;
        }

        private static void Main()
        {
            PrintHeader();
            InitializeConfigs();
            DependencyContainer.Instance.Register<IPacketFactory>(new PluggablePacketFactory());
            DependencyContainer.Instance.Register<IPacketHandler>(new PacketHandler());
            InitializePlugins();
            ClientSession.SetPacketFactory(DependencyContainer.Instance.Get<IPacketFactory>());
            ClientSession.SetPacketHandler(DependencyContainer.Instance.Get<IPacketHandler>());
            Console.WriteLine($"\n\nListening on port {_port}");
            if (RegisterServer())
            {
                Console.WriteLine("Failed to register ServerApi");
                return;
            }
            Server.RunServerAsync(_port, new WorldCryptoFactory()).Wait();
            Server.UnregisterServer();
        }
    }
}