using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using Essentials.Item;
using Essentials.MapManagement;
using Essentials.Teleport;
using SaltyEmu.Commands.Interfaces;
using SaltyEmu.Commands.TypeParsers;
using System;
using System.Threading.Tasks;

namespace Essentials
{
    public class EssentialsPlugin : IPlugin
    {
        private static readonly Logger _logger = Logger.GetLogger<EssentialsPlugin>();
        public PluginEnableTime EnableTime => PluginEnableTime.PostContainerBuild;
        public string Name => "Essentials";

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
            var container = ChickenContainer.Instance.Resolve<ICommandContainer>();
            RegisterTypeParsers(container);

            Task.Run(async () =>
            {
                await RegisterCommands(container);
                //await UnregisterCommands(); <-- this is intended to test command unregistering during runtime.
            });
        }

        private static void RegisterTypeParsers(ICommandContainer container)
        {
            container.AddTypeParser(new MapLayerTypeParser());
            container.AddTypeParser(new PlayerEntityTypeParser());
            container.AddTypeParser(new ItemDtoTypeParser());
        }

        private static async Task RegisterCommands(ICommandContainer container)
        {
            try
            {
                await container.AddModuleAsync<TeleportModule>();
                await container.AddModuleAsync<ButcherModule>();
                await container.AddModuleAsync<ItemModule>();
            }
            catch (Exception e)
            {
                _logger.Debug(e.StackTrace);
            }
        }

        private static async Task UnregisterCommands()
        {
            try
            {
                var container = ChickenContainer.Instance.Resolve<ICommandContainer>();
                await container.RemoveCommandAsync("teleport map", false);
            }
            catch (Exception e)
            {
                _logger.Debug(e.StackTrace);
            }
        }

        public void OnLoad()
        {
        }

        public void ReloadConfig()
        {
        }

        public void SaveConfig()
        {
        }

        public void SaveDefaultConfig()
        {
        }
    }
}