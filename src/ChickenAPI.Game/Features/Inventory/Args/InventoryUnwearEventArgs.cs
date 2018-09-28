using ChickenAPI.Game.Data.TransferObjects.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Inventory.Args
{
    public class InventoryUnwearEventArgs : ChickenEventArgs
    {
        public ItemInstanceDto ItemToUnwear { get; set; }
    }
}