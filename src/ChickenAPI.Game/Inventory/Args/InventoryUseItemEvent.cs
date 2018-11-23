using ChickenAPI.Data.Item;

namespace ChickenAPI.Game.Inventory.Args
{
    public class InventoryUseItemEvent
    {
        public ItemInstanceDto Item { get; set; }
    }
}