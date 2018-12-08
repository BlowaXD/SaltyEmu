using System;
using System.Reflection;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Handlers
{
    public class ItemUpgradeHandler
    {
        private readonly Action<IPlayerEntity, ItemUpgradeEventArgs> _func;

        public ItemUpgradeHandler(MethodInfo method) : this(method.GetCustomAttribute<ItemUpgradeHandlerAttribute>(), method)
        {
        }

        public ItemUpgradeHandler(ItemUpgradeHandlerAttribute attribute, MethodInfo method)
        {
            Type = attribute.Type;

            if (method == null)
            {
                throw new Exception($"Your handler for {Type} is wrong");
            }

            _func = (Action<IPlayerEntity, ItemUpgradeEventArgs>)Delegate.CreateDelegate(typeof(Action<IPlayerEntity, ItemUpgradeEventArgs>), method);
        }

        public UpgradePacketType Type { get; }

        public void Handle(IPlayerEntity player, ItemUpgradeEventArgs e)
        {
            if (e.Type != Type)
            {
                return;
            }

            _func.Invoke(player, e);
        }
    }
}