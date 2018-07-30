using ChickenAPI.ECS.Systems;

namespace ChickenAPI.Game.Systems.Inventory.Args
{
    public class InventoryEqInfoEventArgs : SystemEventArgs
    {

        public byte Type { get; set; } // todo more information

        public short Slot { get; set; }

        public long? ShopOwnerId { get; set; }
    }
}