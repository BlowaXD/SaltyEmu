using System;
using ChickenAPI.Core.Plugins;

namespace SaltyEmu.IpcPlugin
{
    public class Class1 : IPlugin
    {
        public PluginEnableTime EnableTime => PluginEnableTime.PreContainerBuild;
        public string Name { get; }

        public void OnDisable()
        {
            throw new NotImplementedException();
        }

        public void OnEnable()
        {
            throw new NotImplementedException();
        }

        public void OnLoad()
        {
            throw new NotImplementedException();
        }

        public void ReloadConfig()
        {
            throw new NotImplementedException();
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