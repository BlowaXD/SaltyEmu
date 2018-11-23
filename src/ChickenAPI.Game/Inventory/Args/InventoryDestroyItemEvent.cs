using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Args
{
    public class InventoryDestroyItemEvent : ChickenEventArgs
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}