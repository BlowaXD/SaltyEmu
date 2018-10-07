using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums;
using ChickenAPI.Game.Data.AccessLayer.Account;
using ChickenAPI.Game.ECS;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Features.Effects;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Packets;
using NLog;
using NosSharp.BasicAlgorithm;
using SaltyEmu.DatabasePlugin;
using NosSharp.PacketHandler;
using NosSharp.Pathfinder;
using NosSharp.RedisSessionPlugin;
using NosSharp.TemporaryMapPlugins;
using SaltyEmu.BasicPlugin;
using World.Network;
using World.Packets;
using World.Utils;
using Logger = ChickenAPI.Core.Logging.Logger;

namespace World
{
    internal class WorldServer
    {
        private static readonly Logger Log = Logger.GetLogger<WorldServer>();

        private static readonly List<IPlugin> Plugins = new List<IPlugin>
        {
            new BasicAlgorithmPlugin(),
            new RedisPlugin(),
            new NosSharpDatabasePlugin(),
            new TemporaryMapPlugin(),
            new PathfinderPlugin()
        };

        private static void InitializePlugins()
        {
            try
            {
                if (!Directory.Exists("plugins"))
                {
                    Directory.CreateDirectory("plugins");
                }

                foreach (IPlugin plugin in Plugins)
                {
                    plugin.OnLoad();
                }

                Plugins.AddRange(new SimplePluginManager().LoadPlugins(new DirectoryInfo("plugins")));
                foreach (IPlugin plugin in Plugins)
                {
                    plugin.OnEnable();
                }

                ChickenContainer.Builder.Register(s => new SimpleEntityManagerContainer()).As<IEntityManagerContainer>().SingleInstance();
            }
            catch (Exception e)
            {
                Log.Error("Plugins load", e);
            }
        }

        private static void InitializeConfigs()
        {
            Environment.SetEnvironmentVariable("io.netty.allocator.type", "unpooled");
            Environment.SetEnvironmentVariable("io.netty.allocator.maxOrder", "10");
            string port = Environment.GetEnvironmentVariable("SERVER_PORT") ?? "1337";
            if (!int.TryParse(port, out int intPort))
            {
                intPort = 1337;
                Server.Port = intPort;
            }

            string tick = Environment.GetEnvironmentVariable("SERVER_REFRESH_RATE") ?? "10";
            if (!int.TryParse(tick, out int tickRate))
            {
                tickRate = 5;
            }

            Server.Port = intPort;
            Server.Ip = Environment.GetEnvironmentVariable("SERVER_IP") ?? "127.0.0.1";
            Server.WorldGroup = Environment.GetEnvironmentVariable("SERVER_WORLDGROUP") ?? "SaltyNos";
            Server.TickRate = tickRate;
            Log.Info($"TICK-RATE : {Server.TickRate} Hz");
            Log.Info($"WORLDGROUP : {Server.WorldGroup}");
            Log.Info($"IP : {Server.Ip}");
            Log.Info($"PORT : {Server.Port}");
        }

        private static void PrintHeader()
        {
            Console.Title = "SaltyEmu - WORLD";
            const string text = @"
███████╗ █████╗ ██╗  ████████╗██╗   ██╗███████╗███╗   ███╗██╗   ██╗    ██╗    ██╗ ██████╗ ██████╗ ██╗     ██████╗ 
██╔════╝██╔══██╗██║  ╚══██╔══╝╚██╗ ██╔╝██╔════╝████╗ ████║██║   ██║    ██║    ██║██╔═══██╗██╔══██╗██║     ██╔══██╗
███████╗███████║██║     ██║    ╚████╔╝ █████╗  ██╔████╔██║██║   ██║    ██║ █╗ ██║██║   ██║██████╔╝██║     ██║  ██║
╚════██║██╔══██║██║     ██║     ╚██╔╝  ██╔══╝  ██║╚██╔╝██║██║   ██║    ██║███╗██║██║   ██║██╔══██╗██║     ██║  ██║
███████║██║  ██║███████╗██║      ██║   ███████╗██║ ╚═╝ ██║╚██████╔╝    ╚███╔███╔╝╚██████╔╝██║  ██║███████╗██████╔╝
╚══════╝╚═╝  ╚═╝╚══════╝╚═╝      ╚═╝   ╚══════╝╚═╝     ╚═╝ ╚═════╝      ╚══╝╚══╝  ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═════╝ 
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

        private static void InitializeAccounts()
        {
            var acc = ChickenContainer.Instance.Resolve<IAccountService>();
            if (acc.GetByName("admin") != null)
            {
                return;
            }

            var account = new AccountDto
            {
                Authority = AuthorityType.Administrator,
                Email = "admin@chickenapi.com",
                Name = "admin",
                Password = "admin".ToSha512()
            };
            acc.Save(account);
            account = new AccountDto
            {
                Authority = AuthorityType.User,
                Email = "user@chickenapi.com",
                Name = "user",
                Password = "user".ToSha512()
            };
            acc.Save(account);
        }

        private static void Main()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            AppDomain.CurrentDomain.UnhandledException += Exit;
            AppDomain.CurrentDomain.ProcessExit += Exit;
            Console.CancelKeyPress += Exit;
            PrintHeader();
            InitializeLogger();
            InitializeConfigs();
            InitializePlugins();
            ChickenContainer.Builder.Register(s => new PacketHandler()).As<IPacketHandler>().SingleInstance();
            ChickenContainer.Initialize();
            ClientSession.SetPacketFactory(new PluggablePacketFactory());
            ClientSession.SetPacketHandler(ChickenContainer.Instance.Resolve<IPacketHandler>());
            if (Server.RegisterServer())
            {
                Log.Info("Failed to register to ServerAPI");
                return;
            }

#if DEBUG
            InitializeAccounts();
#endif
            InitializeEventHandlers();

            var packetHandler = new PacketHandlerPlugin();
            packetHandler.OnLoad();
            packetHandler.OnEnable();
            Server.RunServerAsync(1337).Wait();
        }

        private static IEnumerable<Type> GetHandlers()
        {
            List<Type> handlers = new List<Type>();

            handlers.AddRange(typeof(EffectEventHandler).Assembly.GetTypes().Where(s => s.IsSubclassOf(typeof(EventHandlerBase))));
            return handlers;
        }

        private static void InitializeEventHandlers()
        {
            // first version hardcoded, next one through Plugin + Assembly Reflection
            var eventManager = ChickenContainer.Instance.Resolve<IEventManager>();
            foreach (Type handler in GetHandlers())
            {
                eventManager.Register(Activator.CreateInstance(handler) as IEventHandler, handler);
            }
        }

        private static void Exit(object sender, EventArgs e)
        {
            Server.UnregisterServer();
            LogManager.Shutdown();
            Console.ReadLine();
        }
    }
}