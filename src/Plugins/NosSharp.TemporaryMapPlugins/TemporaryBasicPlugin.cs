using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Maths;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Features.GuriHandling.Handling;
using ChickenAPI.Game.Features.NpcDialog;
using ChickenAPI.Game.Managers;
using NosSharp.TemporaryMapPlugins;

namespace SaltyEmu.BasicPlugin
{
    public class TemporaryMapPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<TemporaryMapPlugin>();
        public string Name => nameof(TemporaryMapPlugin);

        public void OnDisable()
        {
            // nothing
        }

        public void OnEnable()
        {
            // nothing
        }

        public void OnLoad()
        {
            Log.Info("Loading...");
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