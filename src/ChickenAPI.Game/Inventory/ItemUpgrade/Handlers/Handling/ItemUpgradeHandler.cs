using System;
using System.Reflection;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Handlers.Handling
{
    public class ItemUpgradeHandler
    {
        private readonly Action<IPlayerEntity, ItemUpgradeEvent> _func;

        public ItemUpgradeHandler(ItemUpgradeHandlerAttribute attribute, MethodInfo method)
        {
            Type = attribute.Type;

            if (method == null)
            {
                throw new Exception($"Your handler for {Type} is wrong");
            }

            _func = (Action<IPlayerEntity, ItemUpgradeEvent>)Delegate.CreateDelegate(typeof(Action<IPlayerEntity, ItemUpgradeEvent>), method);
        }

        public UpgradePacketType Type { get; }

        public void Handle(IPlayerEntity player, ItemUpgradeEvent e)
        {
            if (e.Type != Type)
            {
                return;
            }

            _func.Invoke(player, e);
        }
    }
}