using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Inventory.Args
{
    public class InventoryDestroyItemEventArgs : ChickenEventArgs
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}