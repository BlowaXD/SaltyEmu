using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryEqInfoEvent : GameEntityEvent
    {
        public byte Type { get; set; } // todo more information

        public short Slot { get; set; }

        public long? ShopOwnerId { get; set; }
    }
}