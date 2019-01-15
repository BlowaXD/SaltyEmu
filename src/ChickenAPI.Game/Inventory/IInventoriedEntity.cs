namespace ChickenAPI.Game.Inventory
{
    public interface IInventoriedEntity
    {
        InventoryComponent Inventory { get; }
    }
}