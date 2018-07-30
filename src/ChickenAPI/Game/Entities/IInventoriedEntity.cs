using ChickenAPI.Game.Components;

namespace ChickenAPI.Game.Entities
{
    public interface IInventoriedEntity
    {
        InventoryComponent Inventory { get; }
    }
}