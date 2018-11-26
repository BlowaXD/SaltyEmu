using ChickenAPI.Core.Plugins;

namespace Essentials
{
    public class EssentialsPlugin : IPlugin
    {
        public PluginEnableTime EnableTime => PluginEnableTime.PostContainerBuild;
        public string Name => "NosEssentials";

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
            RegisterCommands();
        }

        private void RegisterCommands()
        {
        }

        public void OnLoad()
        {
            throw new System.NotImplementedException();
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