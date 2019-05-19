using System;
using System.IO;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Data.Account;
using ChickenAPI.Data.Server;
using Login.Cryptography.Factories;
using Login.Network;
using SaltyEmu.DatabasePlugin;
using SaltyEmu.RedisWrappers;

namespace Login
{
    internal class LoginServer
    {
        // private static readonly Logger Log = Logger.GetLogger<LoginServer>();
        private static readonly IPluginManager PluginManager = new SimplePluginManager();

        private static ushort _port;

        private static void PrintHeader()
        {
            Console.Title = "SaltyEmu - Login";
            const string text = @"
███████╗ █████╗ ██╗  ████████╗██╗   ██╗███████╗███╗   ███╗██╗   ██╗    ██╗      ██████╗  ██████╗ ██╗███╗   ██╗
██╔════╝██╔══██╗██║  ╚══██╔══╝╚██╗ ██╔╝██╔════╝████╗ ████║██║   ██║    ██║     ██╔═══██╗██╔════╝ ██║████╗  ██║
███████╗███████║██║     ██║    ╚████╔╝ █████╗  ██╔████╔██║██║   ██║    ██║     ██║   ██║██║  ███╗██║██╔██╗ ██║
╚════██║██╔══██║██║     ██║     ╚██╔╝  ██╔══╝  ██║╚██╔╝██║██║   ██║    ██║     ██║   ██║██║   ██║██║██║╚██╗██║
███████║██║  ██║███████╗██║      ██║   ███████╗██║ ╚═╝ ██║╚██████╔╝    ███████╗╚██████╔╝╚██████╔╝██║██║ ╚████║
╚══════╝╚═╝  ╚═╝╚══════╝╚═╝      ╚═╝   ╚══════╝╚═╝     ╚═╝ ╚═════╝     ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝╚═╝  ╚═══╝
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
            // Logger.Initialize();
        }

        private static void InitializeConfiguration()
        {
            string port = Environment.GetEnvironmentVariable("SERVER_PORT");
            if (port == null)
            {
                _port = 4000;
                return;
            }

            _port = Convert.ToUInt16(port);
        }

        private static void InitializePlugins()
        {
            try
            {
                IPlugin[] plugins = PluginManager.LoadPlugins(new DirectoryInfo("plugins"));
                var dbPlugin = new DatabasePlugin();
                dbPlugin.OnLoad();
                var redisPlugin = new RedisPlugin();
                redisPlugin.OnLoad();

                dbPlugin.OnEnable();
                redisPlugin.OnEnable();

                if (plugins == null)
                {
                }
            }
            catch (Exception e)
            {
                // Log.Error("[PLUGINS]", e);
            }
        }

        private static void Main(string[] args)
        {
            PrintHeader();
            InitializeLogger();
            InitializeConfiguration();
            InitializePlugins();
            ChickenContainer.Builder.Register(s => PluginManager).As<IPluginManager>();
            ChickenContainer.Initialize();
            ClientSession.AccountService = ChickenContainer.Instance.Resolve<IAccountService>();
            ClientSession.ServerApi = ChickenContainer.Instance.Resolve<IServerApiService>();
            ClientSession.SessionService = ChickenContainer.Instance.Resolve<ISessionService>();
            NetworkManager.RunServerAsync(_port, new LoginEncoderFactory()).Wait();
        }
    }
}