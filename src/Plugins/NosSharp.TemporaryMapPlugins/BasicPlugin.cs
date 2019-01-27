using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;

namespace SaltyEmu.BasicPlugin
{
    public class BasicPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<BasicPlugin>();
        public PluginEnableTime EnableTime => PluginEnableTime.PostContainerBuild;
        public string Name => nameof(BasicPlugin);

        public void OnDisable()
        {
            // nothing
        }

        public void OnEnable()
        {
            BasicPluginIoCInjector.InitializeEventHandlers();
            BasicPluginIoCInjector.InitializeItemUsageHandlers();
            BasicPluginIoCInjector.InitializeNpcDialogHandlers();
        }

        public void OnLoad()
        {
            Log.Info("Loading...");
            BasicPluginIoCInjector.InjectDependencies();
            Log.Info("Loaded !");
        }

        public void ReloadConfig()
        {
            // nothing
        }

        public void SaveConfig()
        {
            // nothing
        }

        public void SaveDefaultConfig()
        {
            // nothing
        }
    }
}