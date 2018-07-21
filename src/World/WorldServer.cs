using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Autofac;
using ChickenAPI.Data.AccessLayer.Account;
using ChickenAPI.Data.TransferObjects.Character;
using ChickenAPI.Enums;
using ChickenAPI.Packets;
using ChickenAPI.Plugins;
using ChickenAPI.Utils;
using NLog;
using NosSharp.BasicAlgorithm;
using NosSharp.DatabasePlugin;
using NosSharp.PacketHandler;
using NosSharp.RedisSessionPlugin;
using NosSharp.TemporaryMapPlugins;
using World.Network;
using World.Packets;
using World.Utils;
using Logger = ChickenAPI.Utils.Logger;

namespace World
{
    internal class WorldServer
    {
        private static readonly Logger Log = Logger.GetLogger<WorldServer>();
        private static IPlugin[] _plugins;

        private static void InitializePlugins()
        {
            try
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

                var maps = new TemporaryMapPlugin();
                maps.OnLoad();
                maps.OnEnable();

                var algorithm = new BasicAlgorithmPlugin();
                algorithm.OnLoad();
                algorithm.OnEnable();
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
            string tick = Environment.GetEnvironmentVariable("SERVER_REFRESH_RATE") ?? "20";
            if (!int.TryParse(tick, out int tickRate))
            {
                tickRate = 20;
            }

            Server.Port = intPort;
            Server.Ip = Environment.GetEnvironmentVariable("SERVER_IP") ?? "127.0.0.1";
            Server.WorldGroup = Environment.GetEnvironmentVariable("SERVER_WORLDGROUP") ?? "ParaNosia";
            Server.TickRate = tickRate;
            Log.Info($"TICK-RATE : {Server.TickRate} Hz");
            Log.Info($"WORLDGROUP : {Server.WorldGroup}");
            Log.Info($"IP : {Server.Ip}");
            Log.Info($"PORT : {Server.Port}");
        }

        private static void PrintHeader()
        {
            Console.Title = "WingsEmu - WORLD";
            const string text = @" __      __.__                     ___________              
/  \    /  \__| ____    ____  _____\_   _____/ _____  __ __ 
\   \/\/   /  |/    \  / ___\/  ___/|    __)_ /     \|  |  \
 \        /|  |   |  \/ /_/  >___ \ |        \  Y Y  \  |  /
  \__/\  / |__|___|  /\___  /____  >_______  /__|_|  /____/ 
       \/          \//_____/     \/        \/      \/       ";
            int offset = Console.WindowWidth / 2 + text.Length / 2;
            string separator = new string('=', Console.WindowWidth);
            Console.WriteLine(separator + string.Format("{0," + offset + "}\n", text) + separator);
        }

        private static void InitializeLogger()
        {
            Logger.Initialize();
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
            Container.Builder.Register(s => new PacketHandler()).As<IPacketHandler>().SingleInstance();
            Container.Initialize();
            ClientSession.SetPacketFactory(new PluggablePacketFactory());
            ClientSession.SetPacketHandler(Container.Instance.Resolve<IPacketHandler>());
            if (Server.RegisterServer())
            {
                Log.Info($"Failed to register to ServerAPI");
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

            var packetHandler = new PacketHandlerPlugin();
            packetHandler.OnLoad();
            packetHandler.OnEnable();
            Server.RunServerAsync(1337).Wait();
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