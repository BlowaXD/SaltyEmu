using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Handlers.Handling
{
    public interface IItemUpgradeHandler
    {
        void Register(ItemUpgradeHandler handler);

        void Unregister(UpgradePacketType type);

        void Unregister(ItemUpgradeHandlerAttribute handlerAttribute);

        void Execute(IPlayerEntity player, ItemUpgradeEvent @event);
    }
}