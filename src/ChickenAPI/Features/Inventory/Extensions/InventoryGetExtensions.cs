using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Data.TransferObjects.Item;

namespace ChickenAPI.Game.Features.Inventory.Extensions
{
    public static class InventoryGetExtensions
    {


        public static ItemInstanceDto GetItemFromSlotAndType(this InventoryComponent inventory, short itemSlot, InventoryType equipment)
        {
            ItemInstanceDto[] subInv = inventory.GetSubInvFromInventoryType(equipment);
            return itemSlot >= subInv.Length ? null : subInv[itemSlot];
        }
    }
}