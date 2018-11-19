using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Args
{
    public class InventoryUnequipEvent : ChickenEventArgs
    {
        public ItemInstanceDto ItemToUnwear { get; set; }
    }
}