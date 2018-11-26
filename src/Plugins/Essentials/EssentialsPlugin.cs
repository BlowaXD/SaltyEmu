using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Plugins;
using Essentials.Item;
using Essentials.MapManagement;
using Essentials.Teleport;
using SaltyEmu.Commands.Interfaces;

namespace Essentials
{
    public class EssentialsPlugin : IPlugin
    {
        public PluginEnableTime EnableTime => PluginEnableTime.PostContainerBuild;
        public string Name => "Essentials";

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
            RegisterCommands();
        }

        private static void RegisterCommands()
        {
            var container = ChickenContainer.Instance.Resolve<ICommandContainer>();
            container.AddModuleAsync<TeleportModule>();
            container.AddModuleAsync<ButcherModule>();
            container.AddModuleAsync<ItemModule>();
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