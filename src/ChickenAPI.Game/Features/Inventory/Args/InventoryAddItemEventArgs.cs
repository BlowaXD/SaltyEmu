using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Inventory.Args
{
    public class InventoryAddItemEventArgs : ChickenEventArgs
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}