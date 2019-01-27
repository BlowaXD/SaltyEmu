using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class InventoryGetExtensions
    {
        private static readonly IGameConfiguration GameConf = new Lazy<IGameConfiguration>(ChickenContainer.Instance.Resolve<IGameConfiguration>).Value;
        public static ItemInstanceDto GetWeared(this InventoryComponent inv, EquipmentType equipmentType) => inv.Wear[(int)equipmentType];


        public static InvPacket GenerateInventoryPacket(this IPlayerEntity player, InventoryType type)
        {
            ItemInstanceDto[] items = player.Inventory.GetSubInvFromInventoryType(type);

            var packet = new InvPacket
            {
                InventoryType = type,
                Items = new List<string>()
            };

            if (items.All(s => s == null))
            {
                return packet;
            }

            switch (type)
            {
                case InventoryType.Equipment:
                    packet.Items.AddRange(items.Where(s => s != null).Select(s =>
                        $"{s.Slot}.{s.ItemId}.{s.Rarity}.{(s.Item.IsColored && s.Item.EquipmentSlot == EquipmentType.Sp ? s.Design : s.Upgrade)}.{s.SpStoneUpgrade}"));
                    break;

                case InventoryType.Etc:
                case InventoryType.Main:
                    packet.Items.AddRange(items.Where(s => s != null).Select(s =>
                        $"{s.Slot}.{s.ItemId}.{s.Amount}.0"));
                    break;

                case InventoryType.Miniland:
                    packet.Items.AddRange(items.Where(s => s != null).Select(s =>
                        $"{s.Slot}.{s.ItemId}.{s.Amount}"));
                    break;
                case InventoryType.Wear:
                    break;
            }

            return packet;
        }

        public static short GetFirstFreeSlot(this InventoryComponent inv, IReadOnlyCollection<ItemInstanceDto> subInventory, ItemDto source, short amount)
        {
            ItemInstanceDto item = subInventory.FirstOrDefault(x => x != null &&
                x.Amount + amount < GameConf.Inventory.MaxItemPerSlot && x.ItemId == source.Id && x.Item.Type != InventoryType.Equipment);

            return item?.Slot ?? GetFirstFreeSlot(inv, subInventory);
        }

        public static short GetFirstFreeSlot(this InventoryComponent inv, IReadOnlyCollection<ItemInstanceDto> subInventory, ItemInstanceDto source) =>
            GetFirstFreeSlot(inv, subInventory, source.Item, source.Amount);


        public static bool HasItem(this InventoryComponent inv, long itemId)
        {
            return GetItems(inv).Any(s => s.ItemId == itemId);
        }

        public static int GetItemQuantityById(this InventoryComponent inv, long itemId)
        {
            return GetItems(inv).Where(s => s.ItemId == itemId).Sum(s => s.Amount);
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
            return GetItems(inventory, dto => dto != null);
        }

        public static IEnumerable<ItemInstanceDto> GetItems(this InventoryComponent inventory, Func<ItemInstanceDto, bool> predicate)
        {
            List<ItemInstanceDto> list = new List<ItemInstanceDto>();
            list.AddRange(inventory.Wear.Where(predicate));
            list.AddRange(inventory.Costumes.Where(predicate));
            list.AddRange(inventory.Equipment.Where(predicate));
            list.AddRange(inventory.Etc.Where(predicate));
            list.AddRange(inventory.Main.Where(predicate));
            list.AddRange(inventory.Specialists.Where(predicate));
            return list;
        }


        /// <summary>
        ///     Gets the first available slot by it's inventory type
        /// </summary>
        /// <param name="inv"></param>
        /// <param name="inventoryType"></param>
        /// <returns>Returns -1 in case it could not find any available slot</returns>
        public static short GetFirstFreeSlot(this InventoryComponent inv, InventoryType inventoryType) => GetFirstFreeSlot(inv, GetSubInvFromInventoryType(inv, inventoryType));

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