using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.ECS;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Features.GuriHandling.Handling;
using ChickenAPI.Game.Features.NpcDialog;
using ChickenAPI.Game.Managers;
using NosSharp.TemporaryMapPlugins;

namespace SaltyEmu.BasicPlugin
{
    public class BasicPluginIoCInjector
    {
        public static void InjectDependencies()
        {
            ChickenContainer.Builder.Register(s => new LazyMapManager()).As<IMapManager>().SingleInstance();
            ChickenContainer.Builder.Register(c => new SimpleItemInstanceFactory(c.Resolve<IItemService>())).As<IItemInstanceFactory>();
            ChickenContainer.Builder.Register(s => new EventManager()).As<IEventManager>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new RandomGenerator()).As<IRandomGenerator>().SingleInstance();
            ChickenContainer.Builder.Register(s => new BasicNpcDialogHandler()).As<INpcDialogHandler>().SingleInstance();
            ChickenContainer.Builder.Register(s => new BaseGuriHandler()).As<IGuriHandler>().SingleInstance();
            ChickenContainer.Builder.Register(s => new BasicHitRequestFactory()).As<IHitRequestFactory>().SingleInstance();
            ChickenContainer.Builder.Register(s => new SimpleEntityManagerContainer()).As<IEntityManagerContainer>().SingleInstance();
            ChickenContainer.Builder.Register(s => new SimplePlayerManager()).As<IPlayerManager>().SingleInstance();
        }
    }
}