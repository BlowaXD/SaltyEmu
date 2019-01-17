using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FriendsPlugin.Services;

namespace SaltyEmu.FriendsPlugin
{
    public class RelationsPlugin : IPlugin
    {
        public PluginEnableTime EnableTime => PluginEnableTime.PreContainerBuild;
        public string Name => nameof(RelationsPlugin);

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
            /*
             * Inject Server handlers
             */
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