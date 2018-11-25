using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Args
{
    public class InventoryUseItemEvent : ChickenEventArgs
    {
        public ItemInstanceDto Item { get; set; }
    }
}