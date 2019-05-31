using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Core.Plugins.Exceptions;
using ChickenAPI.Core.Utils;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Utils;
using SaltyEmu.Core.Logging;
using SaltyEmu.Core.Plugins;
using SaltyEmu.Redis;

namespace SaltyEmu.RelationService
{
    internal class RelationService
    {
        private static ILogger Log;

        private static readonly IPluginManager PluginManager = new SimplePluginManager();
        //private static readonly Logger Log = Logger.GetLogger("RelationService");

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
            new DatabasePlugin.DatabasePlugin(Logger.GetLogger<DatabasePlugin.DatabasePlugin>()),
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
            ChickenContainer.Builder.Register(s => new Logger(typeof(RelationService))).As<ILogger>();
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

        private static void RegisterDependencies()
        {
            ChickenContainer.Builder.Register(s => new RedisConfiguration { Host = "localhost", Port = 6379 }).As<RedisConfiguration>();
            ChickenContainer.Builder.Register(s => new MqttClientConfigurationBuilder().ConnectTo("localhost").WithName("relation-service-client"));
            ChickenContainer.Builder.Register(s => new MqttServerConfigurationBuilder().ConnectTo("localhost").WithName("relation-service-server"));
            ChickenContainer.Builder.RegisterAssemblyTypes(typeof(RelationService).Assembly).AsClosedTypesOf(typeof(GenericIpcRequestHandler<,>)).PropertiesAutowired();
            ChickenContainer.Builder.RegisterAssemblyTypes(typeof(RelationService).Assembly).AsClosedTypesOf(typeof(GenericAsyncRpcRequestHandler<>)).PropertiesAutowired();
            ChickenContainer.Builder.RegisterAssemblyTypes(typeof(RelationService).Assembly).AsImplementedInterfaces().PropertiesAutowired();
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
            RegisterDependencies();
            CommunicationIocInjector.Inject();
            ChickenContainer.Initialize();
            EnablePlugins(PluginEnableTime.PostContainerBuild);
            var server = ChickenContainer.Instance.Resolve<IRpcServer>();
            var container = ChickenContainer.Instance.Resolve<IIpcPacketHandlersContainer>();
            foreach (Type handlerType in typeof(RelationService).Assembly.GetTypesImplementingGenericClass(typeof(GenericIpcRequestHandler<,>), typeof(GenericAsyncRpcRequestHandler<>)))
            {
                try
                {
                    object handler = ChickenContainer.Instance.Resolve(handlerType);
                    if (!(handler is IIpcPacketHandler postProcessor))
                    {
                        Log.Warn($"{handler}");
                        continue;
                    }

                    Type type = handlerType.BaseType.GenericTypeArguments[0];

                    container.RegisterAsync(postProcessor, type);
                }
                catch (Exception e)
                {
                    Log.Error("[FAIL]", e);
                }
            }

            Console.ReadLine();
        }
    }
}