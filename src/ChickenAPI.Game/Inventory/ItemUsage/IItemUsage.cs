using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;

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
        void RegisterItemUsageCallback(IItemUsageHandler handler);

        void UseItem(IPlayerEntity player, ItemInstanceDto itemInstance);
    }
}