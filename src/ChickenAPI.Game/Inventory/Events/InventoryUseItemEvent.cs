using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryUseItemEvent : ChickenEventArgs
    {
        public ItemInstanceDto Item { get; set; }

        public byte Option { get; set; }
    }
}