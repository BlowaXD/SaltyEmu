using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Maths;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.BCard;
using ChickenAPI.Data.Item;
using ChickenAPI.Game;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.BCards;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.GuriHandling.Handling;
using ChickenAPI.Game.Inventory.ItemUpgrade;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers.Handling;
using ChickenAPI.Game.Inventory.ItemUsage;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.NpcDialog;
using ChickenAPI.Game._ECS;
using ChickenAPI.Game._Network;
using SaltyEmu.BasicPlugin.BCardHandlers;
using SaltyEmu.BasicPlugin.EventHandlers.Battle;
using SaltyEmu.BasicPlugin.EventHandlers.Guri;
using SaltyEmu.BasicPlugin.Implems;
using SaltyEmu.BasicPlugin.ItemUpgradeHandlers;
using SaltyEmu.BasicPlugin.ItemUsageHandlers;
using SaltyEmu.BasicPlugin.NpcDialogHandlers;
using SaltyEmu.Commands;
using SaltyEmu.Commands.Interfaces;

namespace SaltyEmu.BasicPlugin
{
    public class BasicPluginIoCInjector
    {
        private static readonly Logger Log = Logger.GetLogger<BasicPluginIoCInjector>();

        public static void InitializeEventHandlers()
        {
            // first version hardcoded, next one through Plugin + Assembly Reflection
            var eventPipeline = ChickenContainer.Instance.Resolve<IEventPipeline>();

            foreach (Type handlerType in typeof(BasicPlugin).Assembly.GetTypesImplementingGenericClass(typeof(GenericEventPostProcessorBase<>)))
            {
                try
                {
                    if (handlerType == typeof(Battle_ProcessHitRequest_Handler))
                    {
                        Log.Info("NANI ?!");
                    }

                    object handler = ChickenContainer.Instance.Resolve(handlerType);
                    if (!(handler is IEventPostProcessor postProcessor))
                    {
                        continue;
                    }

                    Type type = handlerType.BaseType.GenericTypeArguments[0];

                    eventPipeline.RegisterPostProcessorAsync(postProcessor, type);
                }
                catch (Exception e)
                {
                    Log.Error("WTF????????", e);
                    // ignored
                }
            }
        }

        public static void InjectDependencies()
        {
            ChickenContainer.Builder.Register(_ =>
                    ConfigurationHelper.Load<JsonGameConfiguration>($"plugins/config/{nameof(BasicPlugin)}/rates.json", true))
                .As<IGameConfiguration>().SingleInstance();

            // event handlers
            ChickenContainer.Builder.RegisterAssemblyTypes(typeof(BasicPlugin).Assembly).AsClosedTypesOf(typeof(GenericEventPostProcessorBase<>)).PropertiesAutowired();

            ChickenContainer.Builder.Register(_ => new BasicHitRequestFactory(_.Resolve<IBCardService>())).As<IHitRequestFactory>().InstancePerDependency();

            ChickenContainer.Builder.RegisterType<BasicGameEntityFactory>().AsImplementedInterfaces().PropertiesAutowired();
            ChickenContainer.Builder.RegisterType<LazyMapManager>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();

            ChickenContainer.Builder.Register(_ => new BasicBCardHandlerContainer()).As<IBCardHandlerContainer>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new BasicEventPipelineAsync()).As<IEventPipeline>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new BasicPacketPipelineAsync()).As<IPacketPipelineAsync>().SingleInstance();
            ChickenContainer.Builder.Register(c => new SimpleItemInstanceDtoFactory(c.Resolve<IItemService>())).As<IItemInstanceDtoFactory>().InstancePerDependency();
            ChickenContainer.Builder.Register(_ => new RandomGenerator()).As<IRandomGenerator>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new BasicNpcDialogHandler()).As<INpcDialogHandler>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new BaseGuriHandler()).As<IGuriHandler>().SingleInstance();
            // ChickenContainer.Builder.Register(_ => new BaseUseItemHandler()).As<IItemUsageContainer>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new UseItemHandlerContainer()).As<IItemUsageContainerAsync>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new SimpleEntityManagerContainer()).As<IEntityManagerContainer>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new SimplePlayerManager()).As<IPlayerManager>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new CommandHandler()).As<ICommandContainer>().SingleInstance();
            ChickenContainer.Builder.Register(_ => new BasicUpgradeHandler()).As<IItemUpgradeHandler>().SingleInstance();
        }
    }
}