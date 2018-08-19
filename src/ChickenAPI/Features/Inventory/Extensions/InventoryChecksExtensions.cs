using ChickenAPI.Game.Data.TransferObjects.Item;

namespace ChickenAPI.Game.Features.Inventory.Extensions
{
    public static class InventoryChecksExtensions
    {
        public static bool CanAddItem(this InventoryComponent inv, ItemDto item)
        {
            ItemInstanceDto[] subinv = inv.GetSubInvFromItem(item);

            return inv.GetFirstFreeSlot(subinv) != - 1;
        }

    }
}