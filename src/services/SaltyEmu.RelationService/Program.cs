using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Core.Plugins.Exceptions;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;
using SaltyEmu.Communication.Utils;
using SaltyEmu.Core.Plugins;
using SaltyEmu.FriendsPlugin;
using SaltyEmu.Redis;

namespace SaltyEmu.RelationService
{
    internal class Program
    {
        private static readonly IPluginManager PluginManager = new SimplePluginManager();
        private static readonly Logger Log = Logger.GetLogger("RelationService");

        private static void PrintHeader()
        {
            Console.Title = "SaltyEmu - RelationService";
            const string text = @"
███████╗ █████╗ ██╗  ████████╗██╗   ██╗███████╗███╗   ███╗██╗   ██╗    ███████╗██████╗ ██╗███████╗███╗   ██╗██████╗ ███████╗
██╔════╝██╔══██╗██║  ╚══██╔══╝╚██╗ ██╔╝██╔════╝████╗ ████║██║   ██║    ██╔════╝██╔══██╗██║██╔════╝████╗  ██║██╔══██╗██╔════╝
███████╗███████║██║     ██║    ╚████╔╝ █████╗  ██╔████╔██║██║   ██║    █████╗  ██████╔╝██║█████╗  ██╔██╗ ██║██║  ██║███████╗
╚════██║██╔══██║██║     ██║     ╚██╔╝  ██╔══╝  ██║╚██╔╝██║██║   ██║    ██╔══╝  ██╔══██╗██║██╔══╝  ██║╚██╗██║██║  ██║╚════██║
███████║██║  ██║███████╗██║      ██║   ███████╗██║ ╚═╝ ██║╚██████╔╝    ██║     ██║  ██║██║███████╗██║ ╚████║██████╔╝███████║
╚══════╝╚═╝  ╚═╝╚══════╝╚═╝      ╚═╝   ╚══════╝╚═╝     ╚═╝ ╚═════╝     ╚═╝     ╚═╝  ╚═╝╚═╝╚══════╝╚═╝  ╚═══╝╚═════╝ ╚══════╝
";
            Console.WindowWidth = text.Split('\n')[1].Length + (((text.Split('\n')[1].Length % 2) == 0) ? 1 : 2);
            string separator = new string('=', Console.WindowWidth);
            string logo = text.Split('\n').Select(s => string.Format("{0," + (Console.WindowWidth / 2 + separator.Length / 2) + "}\n", s))
                .Aggregate("", (current, i) => current + i);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(separator + logo + separator);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static readonly List<IPlugin> Plugins = new List<IPlugin>
        {
            new DatabasePlugin.DatabasePlugin(),
        };


        private static bool InitializePlugins()
        {
            try
            {
                ChickenContainer.Builder.Register(s => PluginManager).As<IPluginManager>();
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

        private static void InitializeLogger()
        {
            Logger.Initialize();
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
            PrintHeader();
            InitializeLogger();
            if (InitializePlugins())
            {
                return;
            }

            EnablePlugins(PluginEnableTime.PreContainerBuild);
            ChickenContainer.Initialize();
            EnablePlugins(PluginEnableTime.PostContainerBuild);
            InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Console.ReadLine();
        }


        private static async Task InitializeAsync()
        {
            var conf = new RedisConfiguration();

            MqttServerConfigurationBuilder builder = new MqttServerConfigurationBuilder()
                .ConnectTo("localhost")
                .WithName("relations-server")
                .AddTopic(Configuration.RequestQueue)
                .WithBroadcastTopic(Configuration.BroadcastQueue)
                .WithResponseTopic(Configuration.ResponseQueue)
                .WithSerializer(new JsonSerializer())
                .WithRequestHandler(new RequestHandler());

            RelationServer tmp = await new RelationServer(builder, conf).InitializeAsync();
        }
    }
}