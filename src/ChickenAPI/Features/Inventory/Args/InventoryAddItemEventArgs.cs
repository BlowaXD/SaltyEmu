using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.ECS.Systems.Args;
using ChickenAPI.Game.Data.TransferObjects.Item;

namespace ChickenAPI.Game.Features.Inventory.Args
{
    public class InventoryAddItemEventArgs : SystemEventArgs
    {
        public ItemInstanceDto ItemInstance { get; set; }
    }
}