using ChickenAPI.Core.Events;

namespace ChickenAPI.Game.Features.Inventory.Args
{
    public class InventoryEqInfoEventArgs : ChickenEventArgs
    {
        public byte Type { get; set; } // todo more information

        public short Slot { get; set; }

        public long? ShopOwnerId { get; set; }
    }
}