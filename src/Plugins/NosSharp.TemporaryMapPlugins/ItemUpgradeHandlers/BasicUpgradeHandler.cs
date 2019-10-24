using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.ItemUpgrade;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers.Handling;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.ItemUpgradeHandlers
{
    public class BasicUpgradeHandler : IItemUpgradeHandler
    {
        private readonly ILogger _log;
        protected readonly Dictionary<UpgradePacketType, ItemUpgradeHandler> HandlersByUpgradeType;

        public BasicUpgradeHandler(ILogger log)
        {
            _log = log;
            HandlersByUpgradeType = new Dictionary<UpgradePacketType, ItemUpgradeHandler>();
            Assembly currentAsm = Assembly.GetAssembly(typeof(BasicUpgradeHandler));
            // get types
            foreach (Type type in currentAsm.GetTypes().Where(s => s.GetMethods().Any(m => m.GetCustomAttribute<ItemUpgradeHandlerAttribute>() != null)))
            {
                // each method for a type
                foreach (MethodInfo method in type.GetMethods().Where(s => s.GetCustomAttribute<ItemUpgradeHandlerAttribute>() != null))
                {
                    foreach (ItemUpgradeHandlerAttribute attribute in method.GetCustomAttributes<ItemUpgradeHandlerAttribute>())
                    {
                        Register(new ItemUpgradeHandler(attribute, method));
                    }
                }
            }
        }

        public void Register(ItemUpgradeHandler handlerAttribute)
        {
            if (HandlersByUpgradeType.ContainsKey(handlerAttribute.Type))
            {
                return;
            }

            _log.Info($"[REGISTER_HANDLER] UPGRADE_TYPE : {handlerAttribute.Type} REGISTERED !");
            HandlersByUpgradeType.Add(handlerAttribute.Type, handlerAttribute);
        }

        public void Unregister(UpgradePacketType type)
        {
            HandlersByUpgradeType.Remove(type);
        }

        public void Unregister(ItemUpgradeHandlerAttribute handlerAttribute)
        {
            Unregister(handlerAttribute.Type);
        }

        public void Execute(IPlayerEntity player, ItemUpgradeEvent @event)
        {
            if (!HandlersByUpgradeType.TryGetValue(@event.Type, out ItemUpgradeHandler handler))
            {
                return;
            }

            handler.Handle(player, @event);
        }
    }
}