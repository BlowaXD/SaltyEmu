using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Maths;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Game.Battle.HitRequest;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Data.AccessLayer.Item;
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
            ChickenContainer.Builder.Register(s => new LazyMapManager()).As<IMapManager>().SingleInstance();
            ChickenContainer.Builder.Register(c => new SimpleItemInstanceFactory(c.Resolve<IItemService>())).As<IItemInstanceFactory>();
            ChickenContainer.Builder.Register(s => new EventManager()).As<IEventManager>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new RandomGenerator()).As<IRandomGenerator>().SingleInstance();
            ChickenContainer.Builder.Register(s => new BasicNpcDialogHandler()).As<INpcDialogHandler>().SingleInstance();
            ChickenContainer.Builder.Register(s => new BaseGuriHandler()).As<IGuriHandler>().SingleInstance();
            ChickenContainer.Builder.Register(s => new BasicHitRequestFactory()).As<IHitRequestFactory>().SingleInstance();
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