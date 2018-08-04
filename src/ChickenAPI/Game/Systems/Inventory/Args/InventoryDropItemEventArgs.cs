using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Game.Data.TransferObjects.Item;

namespace ChickenAPI.Game.Game.Systems.Inventory.Args
{
    public class InventoryDropItemEventArgs : SystemEventArgs
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}