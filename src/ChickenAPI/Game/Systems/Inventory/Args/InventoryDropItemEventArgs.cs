using ChickenAPI.Data.TransferObjects.Item;
using ChickenAPI.ECS.Systems;

namespace ChickenAPI.Game.Systems.Inventory.Args
{
    public class InventoryDropItemEventArgs : SystemEventArgs
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}