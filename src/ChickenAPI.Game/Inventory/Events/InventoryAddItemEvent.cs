using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryAddItemEvent : ChickenEventArgs
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}