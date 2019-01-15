using System;
using ChickenAPI.Enums.Packets;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Handlers.Handling
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ItemUpgradeHandlerAttribute : Attribute
    {
        public ItemUpgradeHandlerAttribute(UpgradePacketType type) => Type = type;

        public UpgradePacketType Type { get; }
    }
}