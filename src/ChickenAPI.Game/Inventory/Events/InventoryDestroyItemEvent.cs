using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryDestroyItemEvent : GameEntityEvent
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}