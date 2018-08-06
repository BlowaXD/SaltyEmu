using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.ECS.Systems.Args;

namespace ChickenAPI.Game.Features.Inventory.Args
{
    public class InventoryEqInfoEventArgs : SystemEventArgs
    {
        public byte Type { get; set; } // todo more information

        public short Slot { get; set; }

        public long? ShopOwnerId { get; set; }
    }
}