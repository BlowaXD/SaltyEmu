using ChickenAPI.Data.Item;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryDestroyItemEvent : GameEntityEvent
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}