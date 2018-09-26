using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.Data.TransferObjects.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Features.Effects;
using ChickenAPI.Game.Features.Inventory.Args;
using ChickenAPI.Game.Features.Inventory.Extensions;
using ChickenAPI.Game.Packets;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace ChickenAPI.Game.Features.Inventory
{
    public class InventoryEventHandler : EventHandlerBase
    {
        public override void Execute(IEntity entity, ChickenEventArgs e)
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

                case InventoryWearEventArgs inventoryWear:
                    WearItem(inventory, entity as IPlayerEntity, inventoryWear);
                    break;

                case InventoryUnwearEventArgs inventoryUnwear:
                    UnequipItem(inventory, entity, inventoryUnwear);
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

        private void WearItem(InventoryComponent inventory, IPlayerEntity player, InventoryWearEventArgs inventoryWear)
        {
            ItemInstanceDto item = inventory.GetItemFromSlotAndType(inventoryWear.InventorySlot, InventoryType.Equipment);
            if (item == null)
            {
                return;
            }

            // check shop opened
            // check exchange

            EquipItem(inventory, player, item);
            player.SendPacket(player.GenerateEffectPacket(123));

            if (!(player.EntityManager is IBroadcastable broadcastable))
            {
                return;
            }

            broadcastable.Broadcast(player.GenerateEqPacket());
            player.SendPacket(player.GenerateEquipmentPacket());
            player.SendPacket(player.GenerateStatCharPacket());
            broadcastable.Broadcast(player.GeneratePairyPacket());
        }

        /// <summary>
        ///     Equip the given item
        ///     In case the slot is actually taken, it will swap the items
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        private void EquipItem(InventoryComponent inventory, IEntity entity, ItemInstanceDto item)
        {
            // check if slot already claimed
            ItemInstanceDto tmp = inventory.GetWeared(item.Item.EquipmentSlot);
            var player = entity as IPlayerEntity;

            if (tmp != null)
            {
                // todo refacto to "MoveSlot" method
                inventory.Equipment[item.Slot] = tmp;
                tmp.Slot = item.Slot;
                tmp.Type = InventoryType.Equipment;
            }

            player?.SendPacket(GenerateEmptyIvnPacket(item.Type, item.Slot));
            inventory.Wear[(int)item.Item.EquipmentSlot] = item;
            item.Slot = (short)item.Item.EquipmentSlot;
            item.Type = InventoryType.Wear;

            if (tmp == null)
            {
                return;
            }

            player?.SendPacket(GenerateIvnPacket(tmp));
        }

        private void UnequipItem(InventoryComponent inventory, IEntity entity, InventoryUnwearEventArgs eventArgs)
        {
            short slot = inventory.GetFirstFreeSlot(InventoryType.Equipment);
            if (slot == -1)
            {
                return;
            }

            ItemInstanceDto item = eventArgs.ItemToUnwear;

            inventory.Wear[(int)item.Item.EquipmentSlot] = null;
            inventory.Equipment[slot] = item;
            item.Slot = slot;
            item.Type = InventoryType.Equipment;

            if (!(entity is IPlayerEntity player))
            {
                return;
            }

            player.SendPacket(GenerateIvnPacket(item));

            if (!(player.EntityManager is IBroadcastable broadcastable))
            {
                return;
            }

            broadcastable.Broadcast(player.GenerateEqPacket());
            player.SendPacket(player.GenerateEquipmentPacket());
            player.SendPacket(player.GenerateStatCharPacket());
            broadcastable.Broadcast(player.GeneratePairyPacket());
        }

        private static void InitializeInventory(InventoryComponent inventory, IPlayerEntity player)
        {
            var characterItemService = ChickenContainer.Instance.Resolve<IItemInstanceService>();
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
                    packet.Items.AddRange(items.Where(s => s != null).Select(s =>
                        $"{s.Slot}.{s.ItemId}.{s.Rarity}.{(s.Item.IsColored && s.Item.EquipmentSlot == EquipmentType.Sp ? s.Design : s.Upgrade)}.{s.SpecialistUpgrade2}"));
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
            }

            return packet;
        }

        private static void GenerateInventoryPackets(InventoryComponent inv, IPlayerEntity player)
        {
            player.SendPacket(GenerateInventoryPacket(InventoryType.Equipment, inv.Equipment));
            player.SendPacket(GenerateInventoryPacket(InventoryType.Main, inv.Main));
            player.SendPacket(GenerateInventoryPacket(InventoryType.Etc, inv.Etc));
        }

        private static void AddItem(InventoryComponent inv, InventoryAddItemEventArgs args)
        {
            ItemInstanceDto[] subinv = inv.GetSubInvFromItemInstance(args.ItemInstance);

            short slot = inv.GetFirstFreeSlot(subinv, args.ItemInstance);

            if (slot == -1)
            {
                //Not enough space
                return;
            }

            ItemInstanceDto mergeable = subinv[slot];

            if (mergeable != null)
            {
                mergeable.Amount += args.ItemInstance.Amount;
                args.ItemInstance = mergeable;
            }
            else
            {
                args.ItemInstance.Slot = slot;
                subinv[slot] = args.ItemInstance;
            }

            if (!(inv.Entity is IPlayerEntity player))
            {
                return;
            }

            args.ItemInstance.CharacterId = player.Character.Id;
            player.SendPacket(GenerateIvnPacket(args.ItemInstance));
        }

        private static void DropItem(InventoryComponent inv, InventoryDropItemEventArgs args)
        {
            if (!args.ItemInstance.Item.IsDroppable)
            {
                //Item is not droppable
                return;
            }

            ItemInstanceDto[] subinv = inv.GetSubInvFromItem(args.ItemInstance.Item);

            int itemIndex = Array.FindIndex(subinv, x => x.Slot == args.ItemInstance.Slot);

            subinv[itemIndex] = null;
        }

        private static void DestroyItem(InventoryComponent inv, InventoryDestroyItemEventArgs args)
        {
            ItemInstanceDto[] subinv = inv.GetSubInvFromItemInstance(args.ItemInstance);

            int itemIndex = Array.FindIndex(subinv, x => x.Slot == args.ItemInstance.Slot);

            subinv[itemIndex] = null;
        }

        #region MoveItems

        private static void MoveItem(InventoryComponent inv, InventoryMoveEventArgs args)
        {
            ItemInstanceDto source = inv.GetSubInvFromInventoryType(args.InventoryType)[args.SourceSlot];
            ItemInstanceDto dest = inv.GetSubInvFromInventoryType(args.InventoryType)[args.DestinationSlot];

            if (source == null)
            {
                return;
            }

            if (dest != null && (args.InventoryType == InventoryType.Main || args.InventoryType == InventoryType.Etc) && dest.ItemId == source.ItemId &&
                dest.Amount + source.Amount > InventoryComponent.MAX_ITEM_PER_SLOT)
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
            ItemInstanceDto[] subInv = inv.GetSubInvFromItemInstance(source);
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
            ItemInstanceDto[] subInv = inv.GetSubInvFromItemInstance(source);
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
                    subInv = inventory.GetSubInvFromInventoryType(InventoryType.Wear);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }

                    itemInstance = subInv[eqInfo.Slot];
                    break;
                case 1:
                    subInv = inventory.GetSubInvFromInventoryType(InventoryType.Equipment);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }

                    itemInstance = subInv[eqInfo.Slot];
                    break;
            }

            if (itemInstance == null) return;

            playerEntity.SendPacket(GenerateEInfoPacket(itemInstance));
        }

        private static EInfoPacket GenerateEInfoPacket(ItemInstanceDto itemInstance) => new EInfoPacket();

        #endregion


        #region Utils

        private static IvnPacket GenerateEmptyIvnPacket(InventoryType type, short slot) => new IvnPacket
        {
            InventoryType = type,
            Slot = slot,
            ItemId = -1,
            Upgrade = 0,
            Rare = 0,
            SpStoneUpgrade = 0
        };

        private static IvnPacket GenerateMainIvnPacket(ItemInstanceDto itemInstance) => new IvnPacket
        {
            InventoryType = itemInstance.Type,
            Slot = itemInstance.Slot,
            ItemId = itemInstance.ItemId,
            Rare = itemInstance.Amount,
            Upgrade = 0
        };

        private static IvnPacket GenerateIvnPacket(ItemInstanceDto itemInstance)
        {
            switch (itemInstance.Type)
            {
                case InventoryType.Equipment:
                    return new IvnPacket
                    {
                        InventoryType = itemInstance.Type,
                        ItemId = itemInstance.ItemId,
                        Slot = itemInstance.Slot,
                        Rare = itemInstance.Rarity,
                        Upgrade = itemInstance.Upgrade
                    };
                case InventoryType.Main:
                case InventoryType.Etc:
                    return GenerateMainIvnPacket(itemInstance);
                default: return null;
            }
        }

        #endregion
    }
}