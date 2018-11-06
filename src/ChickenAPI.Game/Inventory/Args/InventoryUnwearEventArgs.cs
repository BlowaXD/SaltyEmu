using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Args
{
    public class InventoryUnwearEventArgs : ChickenEventArgs
    {
        public ItemInstanceDto ItemToUnwear { get; set; }
    }
}