using ChickenAPI.Data.Item;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryUseItemEvent : GameEntityEvent
    {
        public ItemInstanceDto Item { get; set; }

        public byte Option { get; set; }
    }
}