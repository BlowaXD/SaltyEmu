using ChickenAPI.Enums.Packets;
using System;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Handlers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ItemUpgradeHandlerAttribute : Attribute
    {
        public ItemUpgradeHandlerAttribute(UpgradePacketType type)
        {
            Type = type;
        }

        public UpgradePacketType Type { get; }
    }
}