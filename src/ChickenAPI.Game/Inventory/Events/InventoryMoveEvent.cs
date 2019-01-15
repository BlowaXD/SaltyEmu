using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryMoveEvent : GameEntityEvent
    {
        public InventoryType InventoryType { get; set; }

        public short SourceSlot { get; set; }

        public short Amount { get; set; }

        public short DestinationSlot { get; set; }
    }
}