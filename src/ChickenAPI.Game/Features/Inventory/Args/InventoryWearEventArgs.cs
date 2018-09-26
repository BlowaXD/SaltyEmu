using ChickenAPI.Core.Events;
using ChickenAPI.Enums.Game.Items;

namespace ChickenAPI.Game.Features.Inventory.Args
{
    public class InventoryWearEventArgs : ChickenEventArgs
    {
        public short InventorySlot { get; set; }
        public InventoryType InventoryType { get; set; }
    }
}