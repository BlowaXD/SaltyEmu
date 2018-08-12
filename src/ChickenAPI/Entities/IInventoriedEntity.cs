using ChickenAPI.Game.Features.Inventory;

namespace ChickenAPI.Game.Entities
{
    public interface IInventoriedEntity
    {
        InventoryComponent Inventory { get; }
    }
}