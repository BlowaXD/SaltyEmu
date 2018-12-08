using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers;

namespace ChickenAPI.Game.Inventory.ItemUpgrade
{
    public class BasicUpgradeHandler : IItemUpgradeHandler
    {
        private static readonly Logger Log = Logger.GetLogger<BasicUpgradeHandler>();
        protected readonly Dictionary<UpgradePacketType, ItemUpgradeHandler> HandlersByUpgradeType;

        public BasicUpgradeHandler()
        {
            HandlersByUpgradeType = new Dictionary<UpgradePacketType, ItemUpgradeHandler>();
            Assembly currentAsm = Assembly.GetAssembly(typeof(BasicUpgradeHandler));
            // get types
            foreach (Type type in currentAsm.GetTypes().Where(s => s.GetMethods().Any(m => m.GetCustomAttribute<ItemUpgradeHandlerAttribute>() != null)))
            {
                // each method for a type
                foreach (MethodInfo method in type.GetMethods().Where(s => s.GetCustomAttribute<ItemUpgradeHandlerAttribute>() != null))
                {
                    Register(new ItemUpgradeHandler(method));
                }
            }
        }

        public void Register(ItemUpgradeHandler handlerAttribute)
        {
            if (HandlersByUpgradeType.ContainsKey(handlerAttribute.Type))
            {
                return;
            }

            Log.Info($"[REGISTER_HANDLER] UPGRADE_TYPE : {handlerAttribute.Type} REGISTERED !");
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

        public void Execute(IPlayerEntity player, ItemUpgradeEventArgs eventArgs)
        {
            if (!HandlersByUpgradeType.TryGetValue(eventArgs.Type, out ItemUpgradeHandler handler))
            {
                return;
            }

            handler.Handle(player, eventArgs);
        }
    }
}