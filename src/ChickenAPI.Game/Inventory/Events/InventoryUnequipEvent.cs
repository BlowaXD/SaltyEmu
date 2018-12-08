using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryUnequipEvent : ChickenEventArgs
    {
        public ItemInstanceDto ItemToUnwear { get; set; }
    }
}