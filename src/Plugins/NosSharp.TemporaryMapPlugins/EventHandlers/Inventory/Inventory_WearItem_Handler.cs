using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Inventory;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Inventory
{
    public class Inventory_WearItem_Handler : GenericEventPostProcessorBase<InventoryWearEvent>
    {
        protected override async Task Handle(InventoryWearEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            var inventory = player.Inventory;

            ItemInstanceDto item = inventory.GetItemFromSlotAndType(e.InventorySlot, PocketType.Equipment);
            if (item == null)
            {
                return;
            }

            if (e.ItemWearType == ItemWearType.Partner)
            {
                // do the work
                return;
            }

            // check shop opened
            // check exchange

            await EquipItem(inventory, player, item);
            await player.SendPacketAsync(player.GenerateEffectPacket(123));

            await player.BroadcastAsync(player.GenerateEqPacket());
            await player.ActualizeUiWearPanel();
            await player.ActualizeUiStatChar();

            switch (item.Item.EquipmentSlot)
            {
                case EquipmentType.Fairy:
                    await player.BroadcastAsync(player.GeneratePairyPacket());
                    break;

                case EquipmentType.Sp:
                    await player.ActualiseUiSpPoints();
                    break;
            }
        }

        private async Task EquipItem(InventoryComponent inventory, IInventoriedEntity entity, ItemInstanceDto itemToEquip)
        {
            // check if slot already claimed
            ItemInstanceDto alreadyEquipped = inventory.GetWeared(itemToEquip.Item.EquipmentSlot);
            var player = entity as IPlayerEntity;

            if (alreadyEquipped != null)
            {
                // todo refacto to "MoveSlot" method
                inventory.Equipment[itemToEquip.Slot] = alreadyEquipped;
                alreadyEquipped.Slot = itemToEquip.Slot;
                alreadyEquipped.Type = PocketType.Equipment;
            }
            else
            {
                inventory.Equipment[itemToEquip.Slot] = null;
            }

            if (!(player is null))
            {
                await player.SendPacketAsync(player.GenerateEmptyIvnPacket(itemToEquip.Type, itemToEquip.Slot));
            }

            inventory.Wear[(int)itemToEquip.Item.EquipmentSlot] = itemToEquip;
            itemToEquip.Slot = (short)itemToEquip.Item.EquipmentSlot;
            itemToEquip.Type = PocketType.Wear;

            if (alreadyEquipped == null)
            {
                return;
            }

            if (!(player is null))
            {
                await player?.SendPacketAsync(alreadyEquipped.GenerateIvnPacket());
            }
        }
    }
}