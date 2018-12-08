using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Enums.Packets;

namespace ChickenAPI.Game.Inventory.ItemUpgrade
{
    public interface IItemUpgradeHandler
    {
        void Register(ItemUpgradeHandler handler);

        void Unregister(UpgradePacketType Type);

        void Unregister(ItemUpgradeHandlerAttribute handlerAttribute);

        void Execute(IPlayerEntity player, ItemUpgradeEventArgs eventArgs);
    }
}