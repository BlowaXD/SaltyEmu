using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Core.Plugins.Exceptions;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Account;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums;
using ChickenAPI.Game.PacketHandling;
using Essentials;
using NLog;
using NosSharp.PacketHandler;
using SaltyEmu.BasicAlgorithmPlugin;
using SaltyEmu.BasicPlugin;
using SaltyEmu.DatabasePlugin;
using SaltyEmu.PathfinderPlugin;
using SaltyEmu.RedisWrappers;
using World.Network;
using World.Packets;
using World.Utils;
using Logger = ChickenAPI.Core.Logging.Logger;

namespace World
{
    internal class WorldServer
    {
        private static readonly Logger Log = Logger.GetLogger<WorldServer>();
        private static readonly IPluginManager PluginManager = new SimplePluginManager();

        private static readonly List<IPlugin> Plugins = new List<IPlugin>
        {
            new DatabasePlugin(),
            new BasicAlgorithmPlugin(),
            new RedisPlugin(),
            new BasicPlugin(),
            new PathfinderPlugin(),
            new PacketHandlerPlugin(),
            new EssentialsPlugin()
        };


        private static bool InitializePlugins()
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

                Plugins.AddRange(PluginManager.LoadPlugins(new DirectoryInfo("plugins")));
            }
            catch (CriticalPluginException e)
            {
                Log.Error("[PLUGIN]", e);
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Plugins load", e);
            }

            return false;
        }

        private static void InitializeConfigs()
        {
            Environment.SetEnvironmentVariable("io.netty.allocator.type", "unpooled");
            Environment.SetEnvironmentVariable("io.netty.allocator.maxOrder", "10");
            string port = Environment.GetEnvironmentVariable("SERVER_PORT") ?? "1337";
            if (!int.TryParse(port, out int intPort))
            {
                intPort = 1337;
            }

            string tick = Environment.GetEnvironmentVariable("SERVER_REFRESH_RATE") ?? "10";
            if (!int.TryParse(tick, out int tickRate))
            {
                tickRate = 10;
            }

            Server.Port = intPort;
            Server.Ip = Environment.GetEnvironmentVariable("SERVER_IP") ?? "127.0.0.1";
            Server.WorldGroup = Environment.GetEnvironmentVariable("SERVER_WORLDGROUP") ?? "SaltyNos";
            Server.TickRate = tickRate;
            Log.Info($"SERVER_IP : {Server.Ip}");
            Log.Info($"SERVER_PORT : {Server.Port}");
            Log.Info($"SERVER_WORLDGROUP : {Server.WorldGroup}");
            Log.Info($"SERVER_REFRESH_RATE : {Server.TickRate} Hz");
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

        private static void EnablePlugins(PluginEnableTime enableTime)
        {
            Log.Info($"Enabling plugins of type {enableTime}");
            foreach (IPlugin plugin in Plugins.Where(s => s.EnableTime == enableTime))
            {
                plugin.OnEnable();
            }
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
            if (InitializePlugins())
            {
                return;
            }

            EnablePlugins(PluginEnableTime.PreContainerBuild);
            ChickenContainer.Builder.Register(s => PluginManager).As<IPluginManager>();
            ChickenContainer.Initialize();
            InitializeClientSession();
            if (Server.RegisterServer())
            {
                Log.Info("Failed to register to ServerAPI");
                return;
            }

#if DEBUG
            InitializeAccounts();
#endif
            EnablePlugins(PluginEnableTime.PostContainerBuild);
            Server.RunServerAsync(1337).Wait();
        }

        private static void InitializeClientSession()
        {
            ClientSession.SetPacketFactory(new PluggablePacketFactory());
            ClientSession.SetPacketHandler(ChickenContainer.Instance.Resolve<IPacketHandler>());
        }

        private static void Exit(object sender, EventArgs e)
        {
            Server.UnregisterServer();
            LogManager.Shutdown();
            Console.ReadLine();
        }
    }
}