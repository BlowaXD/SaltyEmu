using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Data.AccessLayer.Item;
using ChickenAPI.Data.TransferObjects.Item;
using ChickenAPI.ECS.Entities;
using ChickenAPI.ECS.Systems;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Systems.Inventory.Args;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Game.Server;
using ChickenAPI.Packets.Game.Server.Inventory;
using ChickenAPI.Utils;

namespace ChickenAPI.Game.Systems.Inventory
{
    public class InventorySystem : NotifiableSystemBase
    {
        private const short MAX_AMOUNT_PER_SLOT = 999;

        public InventorySystem(IEntityManager em) : base(em)
        {
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            var inventory = entity.GetComponent<InventoryComponent>();
            if (inventory == null)
            {
                return;
            }

            switch (e)
            {
                case InventoryAddItemEventArgs addItemEventArgs:
                    AddItem(inventory, addItemEventArgs);
                    break;

                case InventoryDropItemEventArgs dropItemEventArgs:
                    DropItem(inventory, dropItemEventArgs);
                    break;

                case InventoryDestroyItemEventArgs destroyItemEventArgs:
                    DestroyItem(inventory, destroyItemEventArgs);
                    break;

                case InventoryGeneratePacketDetailsEventArgs detailsEventArgs:
                    GenerateInventoryPackets(inventory, entity as IPlayerEntity);
                    break;

                case InventoryInitializeEventArgs initEvent:
                    InitializeInventory(inventory, entity as IPlayerEntity);
                    break;

                case InventoryMoveEventArgs moveEvent:
                    MoveItem(inventory, moveEvent);
                    break;
                case InventoryEqInfoEventArgs eqInfo:
                    GetItemInfo(inventory, eqInfo, entity as IPlayerEntity);
                    break;
            }
        }

        private static void InitializeInventory(InventoryComponent inventory, IPlayerEntity player)
        {
            var characterItemService = Container.Instance.Resolve<IItemInstanceService>();
            IEnumerable<ItemInstanceDto> items = characterItemService.GetByCharacterId(player.Character.Id);
            if (items == null || !items.Any())
            {
            }

            foreach (ItemInstanceDto item in items)
            {
                switch (item.Type)
                {
                    case InventoryType.Equipment:
                        inventory.Equipment[item.Slot] = item;
                        break;
                    case InventoryType.Etc:
                        inventory.Etc[item.Slot] = item;
                        break;
                    case InventoryType.Wear:
                        inventory.Wear[item.Slot] = item;
                        break;
                    case InventoryType.Main:
                        inventory.Main[item.Slot] = item;
                        break;
                }
            }
        }

        private static InvPacket GenerateInventoryPacket(InventoryType type, ItemInstanceDto[] items)
        {
            var packet = new InvPacket
            {
                InventoryType = type
            };
            if (items.All(s => s == null))
            {
                return packet;
            }

            switch (type)
            {
                case InventoryType.Equipment:
                    packet.Items.AddRange(items.Select(s =>
                        $"{s.Slot}.{s.ItemId}.{s.Rarity}.{(s.Item.IsColored && s.Item.EquipmentSlot == EquipmentType.Sp ? s.Design : s.Upgrade)}.{s.SpecialistUpgrade2}"));
                    break;
                case InventoryType.Etc:
                case InventoryType.Main:
                    packet.Items.AddRange(items.Select(s =>
                        $"{s.Slot}.{s.ItemId}.{s.Amount}.0"));
                    break;
                case InventoryType.Miniland:
                    packet.Items.AddRange(items.Select(s =>
                        $"{s.Slot}.{s.ItemId}.{s.Amount}"));
                    break;
            }

            return packet;
        }

        private static void GenerateInventoryPackets(InventoryComponent inv, IPlayerEntity player)
        {
            player.SendPacket(GenerateInventoryPacket(InventoryType.Equipment, inv.Equipment));
            player.SendPacket(GenerateInventoryPacket(InventoryType.Main, inv.Main));
            player.SendPacket(GenerateInventoryPacket(InventoryType.Etc, inv.Etc));
            player.SendPacket(GenerateInventoryPacket(InventoryType.Wear, inv.Wear));
        }

        private static void AddItem(InventoryComponent inv, InventoryAddItemEventArgs args)
        {
            ItemInstanceDto[] subinv = GetSubInvFromItemInstance(inv, args.ItemInstance.Item);

            short slot = GetFirstFreeSlot(subinv);

            if (slot == -1)
            {
                //Not enough space
                return;
            }

            args.ItemInstance.Slot = slot;
            subinv[slot] = args.ItemInstance;
            if (!(inv.Entity is IPlayerEntity player))
            {
                return;
            }

            player.SendPacket(GenerateIvnPacket(args.ItemInstance));
        }

        #region MoveItems

        private static void MoveItem(InventoryComponent inv, InventoryMoveEventArgs args)
        {
            ItemInstanceDto source = GetSubInvFromInventoryType(inv, args.InventoryType)[args.SourceSlot];
            ItemInstanceDto dest = GetSubInvFromInventoryType(inv, args.InventoryType)[args.DestinationSlot];

            if (source == null)
            {
                return;
            }

            if (dest != null && (args.InventoryType == InventoryType.Main || args.InventoryType == InventoryType.Etc) && dest.ItemId == source.ItemId &&
                dest.Amount + source.Amount > MAX_AMOUNT_PER_SLOT)
            {
                // if both source & dest are stackable && slots combined are > max slots
                // should provide a "fill" possibility
                return;
            }

            if (dest == null)
            {
                MoveItem(inv, source, args);
            }
            else
            {
                MoveItems(inv, source, dest);
            }
        }

        private static void MoveItem(InventoryComponent inv, ItemInstanceDto source, InventoryMoveEventArgs args)
        {
            ItemInstanceDto[] subInv = GetSubInvFromItemInstance(inv, source);
            subInv[args.DestinationSlot] = source;
            subInv[args.SourceSlot] = null;

            source.Slot = args.DestinationSlot;
            if (!(inv.Entity is IPlayerEntity player))
            {
                return;
            }

            player.SendPacket(GenerateEmptyIvnPacket(args.InventoryType, args.SourceSlot));
            player.SendPacket(GenerateIvnPacket(source));
        }

        private static void MoveItems(InventoryComponent inv, ItemInstanceDto source, ItemInstanceDto dest)
        {
            ItemInstanceDto[] subInv = GetSubInvFromItemInstance(inv, source);
            subInv[dest.Slot] = source;
            subInv[source.Slot] = dest;

            short tmp = dest.Slot;
            dest.Slot = source.Slot;
            source.Slot = tmp;

            if (!(inv.Entity is IPlayerEntity player))
            {
                return;
            }

            player.SendPacket(GenerateIvnPacket(source));
            player.SendPacket(GenerateIvnPacket(dest));
        }

        #endregion

        private static void DropItem(InventoryComponent inv, InventoryDropItemEventArgs args)
        {
            if (!args.ItemInstance.Item.IsDroppable)
            {
                //Item is not droppable
                return;
            }

            ItemInstanceDto[] subinv = GetSubInvFromItemInstance(inv, args.ItemInstance.Item);

            int itemIndex = Array.FindIndex(subinv, x => x.Slot == args.ItemInstance.Slot);

            subinv[itemIndex] = null;
        }

        private static void DestroyItem(InventoryComponent inv, InventoryDestroyItemEventArgs args)
        {
            ItemInstanceDto[] subinv = GetSubInvFromItemInstance(inv, args.ItemInstance.Item);

            int itemIndex = Array.FindIndex(subinv, x => x.Slot == args.ItemInstance.Slot);

            subinv[itemIndex] = null;
        }

        #region ItemInfos

        private static void GetItemInfo(InventoryComponent inventory, InventoryEqInfoEventArgs eqInfo, IPlayerEntity playerEntity)
        {
            if (playerEntity == null)
            {
                return;
            }

            ItemInstanceDto[] subInv;
            ItemInstanceDto itemInstance = null;

            switch (eqInfo.Type)
            {
                case 0:
                    subInv = GetSubInvFromInventoryType(inventory, InventoryType.Wear);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }

                    itemInstance = subInv[eqInfo.Slot];
                    break;
                case 1:
                    subInv = GetSubInvFromInventoryType(inventory, InventoryType.Equipment);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }

                    itemInstance = subInv[eqInfo.Slot];
                    break;
            }

            if (itemInstance == null)
            {
                return;
            }

            playerEntity.SendPacket(GenerateEInfoPacket(itemInstance));
        }

        private static EInfoPacket GenerateEInfoPacket(ItemInstanceDto itemInstance)
        {
            return new EInfoPacket();
        }

        #endregion


        #region Utils

        private static short GetFirstFreeSlot(IReadOnlyCollection<ItemInstanceDto> subinventory)
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

        private static ItemInstanceDto[] GetSubInvFromInventoryType(InventoryComponent inv, InventoryType type)
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

        private static ItemInstanceDto[] GetSubInvFromItemInstance(InventoryComponent inv, ItemDto item)
        {
            return GetSubInvFromInventoryType(inv, item.Type);
        }

        private static ItemInstanceDto[] GetSubInvFromItemInstance(InventoryComponent inv, ItemInstanceDto item)
        {
            return GetSubInvFromInventoryType(inv, item.Type);
        }

        private static IvnPacket GenerateEmptyIvnPacket(InventoryType type, short slot) => new IvnPacket
        {
            InventoryType = type,
            Slot = slot,
            ItemId = -1,
            Upgrade = 0,
            Rare = 0,
            SpStoneUpgrade = 0
        };

        private static IvnPacket GenerateIvnPacket(ItemInstanceDto itemInstance) => new IvnPacket
        {
            InventoryType = itemInstance.Type,
            ItemId = itemInstance.ItemId,
            Slot = itemInstance.Slot,
            Rare = itemInstance.Rarity,
            Upgrade = itemInstance.Upgrade,
            SpStoneUpgrade = itemInstance.SpecialistUpgrade2
        };

        #endregion
    }
}