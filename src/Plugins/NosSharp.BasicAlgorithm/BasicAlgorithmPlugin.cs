using System;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using SaltyEmu.BasicAlgorithmPlugin.IoC;

namespace SaltyEmu.BasicAlgorithmPlugin
{
    public class BasicAlgorithmPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<BasicAlgorithmPlugin>();
        public PluginEnableTime EnableTime => PluginEnableTime.PreContainerBuild;
        public string Name => nameof(BasicAlgorithmPlugin);

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
        }

        public void OnLoad()
        {
            Log.Info("Loading...");
            AlgorithmDependenciesInjector.InjectDependencies();
            Log.Info("Algorithms initialized");
        }

        public void ReloadConfig()
        {
        }

        public void SaveConfig()
        {
            throw new NotImplementedException();
        }

        public void SaveDefaultConfig()
        {
            throw new NotImplementedException();
        }
    }
}