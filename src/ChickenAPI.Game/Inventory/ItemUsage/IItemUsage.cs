using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage.Handling;

namespace ChickenAPI.Game.Inventory.ItemUsage
{
    public interface IItemUsageHandler
    {
        /// <summary>
        /// ItemType for the handler
        /// </summary>
        ItemType Type { get; }

        long EffectId { get; }

        void Handle(IPlayerEntity player, ItemInstanceDto itemInstance);
    }

    public interface IItemUsageContainer
    {
        void RegisterItemUsageCallback(UseItemRequestHandler handler);

        void UseItem(IPlayerEntity player, InventoryUseItemEvent itemInstance);
    }
}