using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Inventory.ItemUsage
{
    public abstract class AItemUsageHandler : IItemUsageHandler
    {
        public ItemType Type { get; }
        public long EffectId { get; }

        public abstract void Handle(IPlayerEntity player, ItemInstanceDto itemInstance);
    }
}