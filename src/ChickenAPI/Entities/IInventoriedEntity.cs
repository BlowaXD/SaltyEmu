using ChickenAPI.Game.Features.Inventory;
using ChickenAPI.Game.Game.Components;

namespace ChickenAPI.Game.Entities
{
    public interface IInventoriedEntity
    {
        InventoryComponent Inventory { get; }
    }
}