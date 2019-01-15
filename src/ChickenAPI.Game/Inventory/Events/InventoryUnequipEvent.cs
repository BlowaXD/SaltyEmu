using ChickenAPI.Data.Item;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryUnequipEvent : GameEntityEvent
    {
        public ItemInstanceDto ItemToUnwear { get; set; }
    }
}