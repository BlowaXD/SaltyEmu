using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game._Events;
using ChickenAPI.Packets.Enumerations;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryMoveEvent : GameEntityEvent
    {
        public PocketType PocketType { get; set; }

        public short SourceSlot { get; set; }

        public short Amount { get; set; }

        public short DestinationSlot { get; set; }
    }
}