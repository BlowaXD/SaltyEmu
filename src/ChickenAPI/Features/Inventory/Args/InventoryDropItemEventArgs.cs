using ChickenAPI.Core.Events;
using ChickenAPI.Game.Data.TransferObjects.Item;

namespace ChickenAPI.Game.Features.Inventory.Args
{
    public class InventoryDropItemEventArgs : ChickenEventArgs
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}