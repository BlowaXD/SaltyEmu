using ChickenAPI.Data.Item;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class InventoryChecksExtensions
    {
        public static bool CanAddItem(this InventoryComponent inv, ItemDto item)
        {
            ItemInstanceDto[] subinv = inv.GetSubInvFromItem(item);

            return inv.GetFirstFreeSlot(subinv, item, 1) != -1;
        }

        public static bool CanAddItem(this InventoryComponent inv, ItemDto item, short amount)
        {
            ItemInstanceDto[] subinv = inv.GetSubInvFromItem(item);

            return inv.GetFirstFreeSlot(subinv, item, amount) != -1;
        }
    }
}