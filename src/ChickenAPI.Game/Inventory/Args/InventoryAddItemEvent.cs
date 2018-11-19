using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Args
{
    public class InventoryAddItemEvent : ChickenEventArgs
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}