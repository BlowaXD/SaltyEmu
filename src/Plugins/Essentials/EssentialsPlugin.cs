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
using Essentials.Character;
using Essentials.Help;

namespace Essentials
{
    public class EssentialsPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<EssentialsPlugin>();
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
            }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static void RegisterTypeParsers(ICommandContainer container)
        {
            container.AddTypeParser(new MapLayerTypeParser());
            container.AddTypeParser(new PlayerEntityTypeParser());
            container.AddTypeParser(new ItemDtoTypeParser());
        }

        private static Task RegisterCommands(ICommandContainer container)
        {
            try
            {
                Task.WaitAll(new[]
                {
                    container.AddModuleAsync<TeleportModule>(),
                    container.AddModuleAsync<ButcherModule>(),
                    container.AddModuleAsync<ItemModule>(),
                    container.AddModuleAsync<CharacterModule>(),
                    container.AddModuleAsync<HelpModule>()
                });
            }
            catch (Exception e)
            {
                Log.Debug(e.StackTrace);
            }

            return Task.CompletedTask;
        }

        private static async Task UnregisterCommands()
        {
            try
            {
                var container = ChickenContainer.Instance.Resolve<ICommandContainer>();
                await container.RemoveCommandsAsync("teleport", false);
            }
            catch (Exception e)
            {
                Log.Debug(e.StackTrace);
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