using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Data.TransferObjects.Item;

namespace ChickenAPI.Game.Features.Inventory.Extensions
{
    public static class InventoryChecksExtensions
    {
        public static bool CanAddItem(this InventoryComponent inv, ItemDto item)
        {
            return true;
        }

        public static short GetFirstFreeSlot(this InventoryComponent inv, IReadOnlyCollection<ItemInstanceDto> subinventory)
        {
            for (int i = 0; i < subinventory.Count; i++)
            {
                ItemInstanceDto item = subinventory.FirstOrDefault(x => x == null || x.Slot == i);

                if (item == null)
                {
                    return (short)i;
                }
            }

            return -1;
        }

        public static ItemInstanceDto[] GetSubInvFromInventoryType(this InventoryComponent inv, InventoryType type)
        {
            switch (type)
            {
                case InventoryType.Wear:
                    return inv.Wear;
                case InventoryType.Equipment:
                    return inv.Equipment;
                case InventoryType.Main:
                    return inv.Main;
                case InventoryType.Etc:
                    return inv.Etc;
                default:
                    return null;
            }
        }

        public static ItemInstanceDto[] GetSubInvFromItem(this InventoryComponent inv, ItemDto item) => GetSubInvFromInventoryType(inv, item.Type);

        public static ItemInstanceDto[] GetSubInvFromItemInstance(this InventoryComponent inv, ItemInstanceDto item) => GetSubInvFromInventoryType(inv, item.Type);

    }
}