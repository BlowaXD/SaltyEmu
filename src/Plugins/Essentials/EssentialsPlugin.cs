using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
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
        }

        public void OnLoad()
        {
            
        }

        public void ReloadConfig()
        {
            throw new System.NotImplementedException();
        }

        public void SaveConfig()
        {
            throw new System.NotImplementedException();
        }

        public void SaveDefaultConfig()
        {
            throw new System.NotImplementedException();
        }
    }
}