using ChickenAPI.Core.Plugins;

namespace SaltyEmu.FamilyPlugin
{
    public class FamilyPlugin : IPlugin
    {
        public PluginEnableTime EnableTime => PluginEnableTime.PreContainerBuild;
        public string Name => nameof(FamilyPlugin);

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
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