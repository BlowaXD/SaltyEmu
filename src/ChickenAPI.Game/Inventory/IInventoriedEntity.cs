using ChickenAPI.Game.Inventory;

namespace ChickenAPI.Game.Entities
{
    public interface IInventoriedEntity
    {
        InventoryComponent Inventory { get; }
    }
}