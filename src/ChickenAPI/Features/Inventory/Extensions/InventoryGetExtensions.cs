using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Data.TransferObjects.Item;

namespace ChickenAPI.Game.Features.Inventory.Extensions
{
    public static class InventoryGetExtensions
    {
        public static ItemInstanceDto GetWeared(this InventoryComponent inv, EquipmentType equipmentType)
        {
            return inv.Wear[(int)equipmentType];
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

        public static IEnumerable<ItemInstanceDto> GetItems(this InventoryComponent inventory)
        {
            List<ItemInstanceDto> list = new List<ItemInstanceDto>();
            list.AddRange(inventory.Wear.Where(s => s != null));
            list.AddRange(inventory.Costumes.Where(s => s != null));
            list.AddRange(inventory.Equipment.Where(s => s != null));
            list.AddRange(inventory.Etc.Where(s => s != null));
            list.AddRange(inventory.Main.Where(s => s != null));
            list.AddRange(inventory.Specialists.Where(s => s != null));
            return list;
        }

        public static IEnumerable<ItemInstanceDto> GetItems(this InventoryComponent inventory, Func<ItemInstanceDto, bool> predicate)
        {
            return GetItems(inventory).Where(predicate);
        }


        /// <summary>
        /// Gets the first available slot by it's inventory type
        /// </summary>
        /// <param name="inv"></param>
        /// <param name="inventoryType"></param>
        /// <returns>Returns -1 in case it could not find any available slot</returns>
        public static short GetFirstFreeSlot(this InventoryComponent inv, InventoryType inventoryType)
        {
            return GetFirstFreeSlot(inv, GetSubInvFromInventoryType(inv, inventoryType));
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


        public static ItemInstanceDto GetItemFromSlotAndType(this InventoryComponent inventory, short itemSlot, InventoryType equipment)
        {
            ItemInstanceDto[] subInv = inventory.GetSubInvFromInventoryType(equipment);
            return itemSlot >= subInv.Length ? null : subInv[itemSlot];
        }
    }
}