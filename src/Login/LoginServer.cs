using System;
using System.IO;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.AccessLayer.Account;
using ChickenAPI.Data.AccessLayer.Server;
using ChickenAPI.Plugins;
using LoginServer.Cryptography.Factories;
using LoginServer.Network;
using NosSharp.DatabasePlugin;
using NosSharp.RedisSessionPlugin;

namespace LoginServer
{
    internal class LoginServer
    {
        private static readonly Logger Log = Logger.GetLogger<LoginServer>();

        private static void PrintHeader()
        {
            Console.Title = "Nos# - LoginServer";
            const string text = "LOGIN SERVER - Nos#";
            int offset = Console.WindowWidth / 2 + text.Length / 2;
            string separator = new string('=', Console.WindowWidth);
            Console.WriteLine(separator + string.Format("{0," + offset + "}\n", text) + separator);
        }

        private static void InitializeLogger()
        {
            Logger.Initialize();
        }

        private static void InitializeConfiguration()
        {
            string port = Environment.GetEnvironmentVariable("PORT");
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
                IPlugin[] plugins = new SimplePluginManager().LoadPlugins(new DirectoryInfo("plugins"));
                var dbPlugin = new NosSharpDatabasePlugin();
                dbPlugin.OnLoad();
                var redisPlugin = new RedisPlugin();
                redisPlugin.OnLoad();

                dbPlugin.OnEnable();
                redisPlugin.OnEnable();

                if (plugins == null)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Log.Error("[PLUGINS]", e);
            }
        }

        private static ushort _port;

        private static void Main(string[] args)
        {
            PrintHeader();
            InitializeLogger();
            InitializeConfiguration();
            InitializePlugins();
            Container.Initialize();
            ClientSession.AccountService = Container.Instance.Resolve<IAccountService>();
            ClientSession.ServerApi = Container.Instance.Resolve<IServerApiService>();
            ClientSession.SessionService = Container.Instance.Resolve<ISessionService>();
            NetworkManager.RunServerAsync(_port, new LoginEncoderFactory()).Wait();
        }
    }
}