using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUsage;
using ChickenAPI.Packets.Game.Server.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Player.Extension;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Entities.Drop;
using ChickenAPI.Game.Entities.Drop.Extensions;
using ChickenAPI.Core.Maths;

namespace ChickenAPI.Game.Inventory
{
    public class InventoryEventHandler : EventHandlerBase
    {
        private static readonly Logger Log = Logger.GetLogger<InventoryEventHandler>();

        private static readonly IItemUsageContainer _ItemUsageHandler = new Lazy<IItemUsageContainer>(() => ChickenContainer.Instance.Resolve<IItemUsageContainer>()).Value;

        private static IRandomGenerator Random => new Lazy<IRandomGenerator>(() => ChickenContainer.Instance.Resolve<IRandomGenerator>()).Value;
        private static IPathfinder PathFinder => new Lazy<IPathfinder>(() => ChickenContainer.Instance.Resolve<IPathfinder>()).Value;
        private static IItemInstanceDtoFactory ItemFactory => new Lazy<IItemInstanceDtoFactory>(() => ChickenContainer.Instance.Resolve<IItemInstanceDtoFactory>()).Value;

        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(InventoryAddItemEvent),
            typeof(InventoryRemoveItemEvent),
            typeof(InventoryDestroyItemEvent),
            typeof(InventoryEqInfoEventArgs),
            typeof(InventoryLoadEvent),
            typeof(InventoryMoveEventArgs),
            typeof(InventoryUnequipEvent),
            typeof(InventoryWearEvent),
            typeof(InventoryUseItemEvent),
            typeof(InventoryPickUpEvent)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            if (!(entity is IInventoriedEntity inventoried))
            {
                return;
            }

            InventoryComponent inventory = inventoried.Inventory;
            if (inventory == null)
            {
                return;
            }

            switch (e)
            {
                case InventoryAddItemEvent addItemEventArgs:
                    AddItem(inventory, addItemEventArgs);
                    break;

                case InventoryRemoveItemEvent dropItemEventArgs:
                    DropItem(inventory, dropItemEventArgs, entity as IPlayerEntity);
                    break;

                case InventoryDestroyItemEvent destroyItemEventArgs:
                    DestroyItem(inventory, destroyItemEventArgs);
                    break;

                case InventoryRequestDetailsEvent detailsEventArgs:
                    GenerateInventoryPackets(inventory, entity as IPlayerEntity);
                    break;

                case InventoryWearEvent inventoryWear:
                    WearItem(inventory, entity as IPlayerEntity, inventoryWear);
                    break;

                case InventoryUnequipEvent inventoryUnwear:
                    UnequipItem(inventory, entity, inventoryUnwear);
                    break;

                case InventoryLoadEvent initEvent:
                    InitializeInventory(inventory, entity as IPlayerEntity);
                    break;

                case InventoryMoveEventArgs moveEvent:
                    MoveItem(inventory, moveEvent);
                    break;

                case InventoryEqInfoEventArgs eqInfo:
                    GetItemInfo(inventory, eqInfo, entity as IPlayerEntity);
                    break;

                case InventoryUseItemEvent item:
                    _ItemUsageHandler.UseItem(entity as IPlayerEntity, item);
                    break;

                case InventoryPickUpEvent pickUpEvent:
                    PickUpItem(inventory, pickUpEvent, entity as IPlayerEntity);
                    break;
            }
        }

        private void PickUpItem(InventoryComponent inv, InventoryPickUpEvent args, IPlayerEntity player)
        {
            ItemInstanceDto[] subinv = inv.GetSubInvFromItem(args.Drop.Item);
            short slot = inv.GetFirstFreeSlot(subinv, args.Drop.Item, (short)args.Drop.Quantity);
            if (slot == -1)
            {
                Log.Info("No available slot");
                return;
            }

            ItemInstanceDto itemInstance = args.Drop.ItemInstance;
            if (itemInstance == null) // Monster Drop
            {
                itemInstance = ItemFactory.CreateItem(args.Drop.Item, (short) args.Drop.Quantity);
            }
            AddItem(inv, new InventoryAddItemEvent() {ItemInstance = itemInstance});
            player.Broadcast(player.GenerateGetPacket(args.Drop.Id));
            args.Drop.CurrentMap.UnregisterEntity(args.Drop);
            args.Drop.Dispose();
        }

        private void WearItem(InventoryComponent inventory, IPlayerEntity player, InventoryWearEvent e)
        {
            ItemInstanceDto item = inventory.GetItemFromSlotAndType(e.InventorySlot, InventoryType.Equipment);
            if (item == null)
            {
                return;
            }

            // check shop opened
            // check exchange

            EquipItem(inventory, player, item);
            player.SendPacket(player.GenerateEffectPacket(123));

            player.Broadcast(player.GenerateEqPacket());
            player.SendPacket(player.GenerateEquipmentPacket());
            player.SendPacket(player.GenerateStatCharPacket());

            switch (item.Item.EquipmentSlot)
            {
                case EquipmentType.Fairy:
                    player.Broadcast(player.GeneratePairyPacket());
                    break;

                case EquipmentType.Sp:
                    player.ActualiseUiSpPoints();
                    break;
            }
        }

        /// <summary>
        ///     Equip the given item
        ///     In case the slot is actually taken, it will swap the items
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="entity"></param>
        /// <param name="itemToEquip"></param>
        private static void EquipItem(InventoryComponent inventory, IEntity entity, ItemInstanceDto itemToEquip)
        {
            // check if slot already claimed
            ItemInstanceDto alreadyEquipped = inventory.GetWeared(itemToEquip.Item.EquipmentSlot);
            var player = entity as IPlayerEntity;

            if (alreadyEquipped != null)
            {
                // todo refacto to "MoveSlot" method
                inventory.Equipment[itemToEquip.Slot] = alreadyEquipped;
                alreadyEquipped.Slot = itemToEquip.Slot;
                alreadyEquipped.Type = InventoryType.Equipment;
            }
            else
            {
                inventory.Equipment[itemToEquip.Slot] = null;
            }

            player?.SendPacket(player.GenerateEmptyIvnPacket(itemToEquip.Type, itemToEquip.Slot));
            inventory.Wear[(int)itemToEquip.Item.EquipmentSlot] = itemToEquip;
            itemToEquip.Slot = (short)itemToEquip.Item.EquipmentSlot;
            itemToEquip.Type = InventoryType.Wear;

            if (alreadyEquipped == null)
            {
                return;
            }

            player?.SendPacket(alreadyEquipped.GenerateIvnPacket());
        }

        private void UnequipItem(InventoryComponent inventory, IEntity entity, InventoryUnequipEvent @event)
        {
            short slot = inventory.GetFirstFreeSlot(InventoryType.Equipment);
            if (slot == -1)
            {
                return;
            }

            ItemInstanceDto item = @event.ItemToUnwear;

            inventory.Wear[(int)item.Item.EquipmentSlot] = null;
            inventory.Equipment[slot] = item;
            item.Slot = slot;
            item.Type = InventoryType.Equipment;

            if (!(entity is IPlayerEntity player))
            {
                return;
            }

            player.SendPacket(item.GenerateIvnPacket());

            player.Broadcast(player.GenerateEqPacket());
            player.SendPacket(player.GenerateEquipmentPacket());
            player.SendPacket(player.GenerateStatCharPacket());
            player.Broadcast(player.GeneratePairyPacket());
        }

        private static void InitializeInventory(InventoryComponent inventory, IPlayerEntity player)
        {
            var characterItemService = ChickenContainer.Instance.Resolve<IItemInstanceService>();
            IEnumerable<ItemInstanceDto> items = characterItemService.GetByCharacterId(player.Character.Id);
            if (items == null || !items.Any())
            {
                return;
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
            }

            return packet;
        }

        private static void GenerateInventoryPackets(InventoryComponent inv, IPlayerEntity player)
        {
            player.SendPacket(GenerateInventoryPacket(InventoryType.Equipment, inv.Equipment));
            player.SendPacket(GenerateInventoryPacket(InventoryType.Main, inv.Main));
            player.SendPacket(GenerateInventoryPacket(InventoryType.Etc, inv.Etc));
        }

        private static void AddItem(InventoryComponent inv, InventoryAddItemEvent args)
        {
            ItemInstanceDto[] subinv = inv.GetSubInvFromItemInstance(args.ItemInstance);

            short slot = inv.GetFirstFreeSlot(subinv, args.ItemInstance);

            if (slot == -1)
            {
                Log.Info("No available slot");
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
            player.SendPacket(args.ItemInstance.GenerateIvnPacket());
        }

        private static void DropItem(InventoryComponent inv, InventoryRemoveItemEvent args, IPlayerEntity player)
        {
            if (args.ItemInstance?.Item.IsDroppable == false)
            {
                //Item is not droppable
                return;
            }

            ItemInstanceDto[] subinv = inv.GetSubInvFromItem(args.ItemInstance.Item);
            int itemIndex = Array.FindIndex(subinv, x => x.Slot == args.ItemInstance.Slot);

            Position<short>[] pos = PathFinder.GetNeighbors(player.Position, player.CurrentMap.Map);
            IDropEntity drop = new ItemDropEntity(player.CurrentMap.GetNextId())
            {
                ItemVnum = args.ItemInstance.ItemId,
                Item = args.ItemInstance.Item,
                ItemInstance = args.ItemInstance,
                DroppedTimeUtc = DateTime.Now,
                Position = pos.Length > 1 ? pos[Random.Next(pos.Length)] : player.Position,
                Quantity = args.Amount
            };
            drop.TransferEntity(player.CurrentMap);
            player.CurrentMap.Broadcast(drop.GenerateDropPacket());

            if (args.Amount >= subinv[itemIndex].Amount)
            {
                subinv[itemIndex] = null;
                player.SendPacket(player.GenerateEmptyIvnPacket(args.ItemInstance.Type, args.ItemInstance.Slot));
                return;
            }
            subinv[itemIndex].Amount -= args.Amount;
            player.SendPacket(subinv[itemIndex].GenerateIvnPacket());
        }

        private static void DestroyItem(InventoryComponent inv, InventoryDestroyItemEvent args)
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

            player.SendPacket(player.GenerateEmptyIvnPacket(args.InventoryType, args.SourceSlot));
            player.SendPacket(source.GenerateIvnPacket());
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

            player.SendPacket(source.GenerateIvnPacket());
            player.SendPacket(dest.GenerateIvnPacket());
        }

        #endregion MoveItems

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

                case 7:
                case 10:
                    subInv = inventory.GetSubInvFromInventoryType(InventoryType.Specialist);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }

                    itemInstance = subInv[eqInfo.Slot];
                    break;

                case 11:
                    subInv = inventory.GetSubInvFromInventoryType(InventoryType.Costume);
                    if (eqInfo.Slot > subInv.Length)
                    {
                        return;
                    }
                    break;
            }

            if (itemInstance == null)
            {
                return;
            }

            if (itemInstance.Item.ItemType == ItemType.Specialist)
            {
                playerEntity.SendPacket(itemInstance.GenerateSlInfoPacket());
                return;
            }

            playerEntity.SendPacket(itemInstance.GenerateEInfoPacket());
        }

        #endregion ItemInfos

        #region Utils

        #endregion Utils
    }
}