using System;
using System.IO;
using System.Reflection;
using Autofac;
using ChickenAPI.DAL.Interfaces;
using ChickenAPI.Dtos;
using ChickenAPI.Enums;
using ChickenAPI.Packets;
using ChickenAPI.Plugin;
using ChickenAPI.Utils;
using log4net;
using log4net.Config;
using log4net.Repository;
using NosSharp.DatabasePlugin;
using NosSharp.PacketHandler;
using NosSharp.RedisSessionPlugin;
using NosSharp.World.Network;
using NosSharp.World.Packets;
using NosSharp.World.Utils;

namespace NosSharp.World
{
    internal class WorldServer
    {
        private static IPlugin[] _plugins;
        private static void InitializePlugins()
        {
            _plugins = new SimplePluginManager().LoadPlugins(new DirectoryInfo("plugins"));
            foreach (IPlugin plugin in _plugins)
            {
                plugin.OnEnable();
            }
            var tmp = new RedisPlugin();
            tmp.OnLoad();
            tmp.OnEnable();
            var tmpAgain = new NosSharpDatabasePlugin();
            tmpAgain.OnLoad();
            tmpAgain.OnEnable();
        }

        private static void InitializeConfigs()
        {
            Environment.SetEnvironmentVariable("io.netty.allocator.type", "unpooled");
            Environment.SetEnvironmentVariable("io.netty.allocator.maxOrder", "5");
            string port = Environment.GetEnvironmentVariable("SERVER_PORT") ?? "1337";
            if (!int.TryParse(port, out int intPort))
            {
                Server.Port = 1337;
            }

            Server.Port = intPort;
            Server.Ip = Environment.GetEnvironmentVariable("SERVER_IP") ?? "127.0.0.1";
            Server.WorldGroup = Environment.GetEnvironmentVariable("SERVER_WORLDGROUP") ?? "NosWings";
        }

        private static void PrintHeader()
        {
            Console.Title = "Nos# - WORLD";
            const string text = "WORLD SERVER - Nos#";
            int offset = Console.WindowWidth / 2 + text.Length / 2;
            string separator = new string('=', Console.WindowWidth);
            Console.WriteLine(separator + string.Format("{0," + offset + "}\n", text) + separator);
        }

        private static void InitializeLogger()
        {
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("plugins/config/log4net.config"));
            Logger.Log = LogManager.GetLogger(typeof(WorldServer));
        }


        private static void Main()
        {
            PrintHeader();
            InitializeLogger();
            InitializeConfigs();
            InitializePlugins();
            Container.Builder.Register(s => new Packets.PacketHandler()).As<IPacketHandler>().SingleInstance();
            Container.Initialize();
            ClientSession.SetPacketFactory(new PluggablePacketFactory());
            ClientSession.SetPacketHandler(Container.Instance.Resolve<IPacketHandler>());
            if (Server.RegisterServer())
            {
                Logger.Log.Info($"Failed to register to ServerAPI");
                return;
            }

            var acc = Container.Instance.Resolve<IAccountService>();
            if (acc.GetByName("admin") == null)
            {
                var account = new AccountDto
                {
                    Authority = AuthorityType.Administrator,
                    Email = "admin@chickenapi.com",
                    Name = "admin",
                    Password = "admin".ToSha512()
                };
                acc.InsertOrUpdate(account);
                account = new AccountDto
                {
                    Authority = AuthorityType.User,
                    Email = "user@chickenapi.com",
                    Name = "user",
                    Password = "user".ToSha512()
                };
                acc.InsertOrUpdate(account);
            }

            var packetHandler = new PacketHandlerPlugin();
            packetHandler.OnLoad();
            packetHandler.OnEnable();
            Server.RunServerAsync(1337).Wait();
            Server.UnregisterServer();
            Console.ReadLine();
        }
    }
}