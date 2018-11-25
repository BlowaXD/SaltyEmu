using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.ECS;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.GuriHandling.Handling;
using ChickenAPI.Game.Inventory.ItemUsage;
using ChickenAPI.Game.Inventory.ItemUsage.Handling;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.NpcDialog;
using ChickenAPI.Game.PacketHandling;
using SaltyEmu.Commands;

namespace SaltyEmu.BasicPlugin
{
    public class BasicPluginIoCInjector
    {
        public static IEnumerable<Type> GetHandlers()
        {
            List<Type> handlers = new List<Type>();

            handlers.AddRange(typeof(EffectEventHandler).Assembly.GetTypes().Where(s => s.IsSubclassOf(typeof(EventHandlerBase))));
            return handlers;
        }

        public static void InitializeEventHandlers()
        {
            // first version hardcoded, next one through Plugin + Assembly Reflection
            var eventManager = ChickenContainer.Instance.Resolve<IEventManager>();
            foreach (Type handlerType in GetHandlers())
            {
                object handler = Activator.CreateInstance(handlerType);

                if (!(handler is EventHandlerBase handlerBase))
                {
                    continue;
                }

                foreach (Type type in handlerBase.HandledTypes)
                {
                    eventManager.Register(handlerBase, type);
                }
            }
        }

        public static void InjectDependencies()
        {
            ChickenContainer.Builder.Register(_ => new LazyMapManager()).As<IMapManager>().SingleInstance();
            ChickenContainer.Builder.Register(c => new SimpleItemInstanceFactory(c.Resolve<IItemService>())).As<IItemInstanceFactory>();
            ChickenContainer.Builder.Register(_ => new EventManager()).As<IEventManager>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new RandomGenerator()).As<IRandomGenerator>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new BasicNpcDialogHandler()).As<INpcDialogHandler>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new BaseGuriHandler()).As<IGuriHandler>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new BaseUseItemHandler()).As<IItemUsageContainer>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new BasicHitRequestFactory()).As<IHitRequestFactory>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new SimpleEntityManagerContainer()).As<IEntityManagerContainer>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new SimplePlayerManager()).As<IPlayerManager>().SingleInstance();
            ChickenContainer.Builder.Register(s => new PacketHandler()).As<IPacketHandler>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new CommandHandler()).SingleInstance();
        }
    }
}