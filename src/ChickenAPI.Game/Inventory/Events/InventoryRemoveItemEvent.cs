using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryRemoveItemEvent : GameEntityEvent
    {
        public ItemInstanceDto ItemInstance { get; set; }

        public short Amount { get; set; }
    }
}