using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Autofac;
using ChickenAPI.Core.ECS;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Core.Utils;
using ChickenAPI.Enums;
using ChickenAPI.Game.Data.AccessLayer.Account;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Features.Chat;
using ChickenAPI.Game.Features.Effects;
using ChickenAPI.Game.Features.Groups;
using ChickenAPI.Game.Features.Inventory;
using ChickenAPI.Game.Features.Quicklist;
using ChickenAPI.Game.Features.Shops;
using ChickenAPI.Game.Features.Visibility;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Packets;
using NLog;
using NosSharp.BasicAlgorithm;
using NosSharp.DatabasePlugin;
using NosSharp.PacketHandler;
using NosSharp.Pathfinder;
using NosSharp.RedisSessionPlugin;
using NosSharp.TemporaryMapPlugins;
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
#if DEBUG
            SetConsoleCtrlHandler(ConsoleCtrlCheck, true);
#endif
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

        private static void InitializeEventHandlers()
        {
            // first version hardcoded, next one through Plugin + Assembly Reflection
            var eventManager = ChickenContainer.Instance.Resolve<IEventManager>();
            eventManager.Register(new EffectEventHandler());
            eventManager.Register(new ChatEventHandler());
            eventManager.Register(new GroupEventHandler());
            eventManager.Register(new InventoryEventHandler());
            eventManager.Register(new VisibilityEventHandler());
            eventManager.Register(new ShopEventHandler());
            eventManager.Register(new QuickListEventHandler());
        }

        private static void Exit(object sender, EventArgs e)
        {
            Server.UnregisterServer();
            LogManager.Shutdown();
            Console.ReadLine();
        }


        // Declare the SetConsoleCtrlHandler function
        // as external and receiving a delegate.

#if DEBUG


        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            // Put your own handler here

            switch (ctrlType)
            {
                case CtrlTypes.CTRL_CLOSE_EVENT:
                    Exit(null, null);
                    break;
                default:
                    Exit(null, null);
                    break;
            }

            return true;
        }

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, bool add);

        // A delegate type to be used as the handler routine
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes ctrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.

        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }
#endif
    }
}