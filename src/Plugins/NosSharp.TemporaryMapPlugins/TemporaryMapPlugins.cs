using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.Managers;

namespace NosSharp.TemporaryMapPlugins
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
            Container.Builder.Register(s => new LazyMapManager()).As<IMapManager>().SingleInstance();
            Container.Builder.Register(c => new SimpleItemInstanceFactory(c.Resolve<IItemService>())).As<IItemInstanceFactory>();
            Container.Builder.Register(s => new EventManager()).As<IEventManager>().SingleInstance();
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