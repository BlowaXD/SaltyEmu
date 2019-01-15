using ChickenAPI.Data.Item;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryAddItemEvent : GameEntityEvent
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}