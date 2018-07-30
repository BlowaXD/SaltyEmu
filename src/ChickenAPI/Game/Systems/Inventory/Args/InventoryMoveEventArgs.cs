using ChickenAPI.ECS.Systems;
using ChickenAPI.Enums.Game.Items;

namespace ChickenAPI.Game.Systems.Inventory.Args
{
    public class InventoryMoveEventArgs : SystemEventArgs
    {
        public InventoryType InventoryType { get; set; }

        public short SourceSlot { get; set; }

        public short Amount { get; set; }

        public short DestinationSlot { get; set; }

    }
}