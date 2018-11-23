using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Args
{
    public class InventoryWearEvent : ChickenEventArgs
    {
        public short InventorySlot { get; set; }
        public InventoryType InventoryType { get; set; }
    }
}