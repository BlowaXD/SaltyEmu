using ChickenAPI.Core.Events;
using ChickenAPI.Game.Data.TransferObjects.Item;

namespace ChickenAPI.Game.Features.Inventory.Args
{
    public class InventoryUnwearEventArgs : ChickenEventArgs
    {
        public ItemInstanceDto ItemToUnwear { get; set; }
    }
}