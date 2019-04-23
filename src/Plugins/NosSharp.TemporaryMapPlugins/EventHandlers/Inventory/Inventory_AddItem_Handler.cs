using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Inventory;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Inventory
{
    public class Inventory_AddItem_Handler : GenericEventPostProcessorBase<InventoryAddItemEvent>
    {
        protected override async Task Handle(InventoryAddItemEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IInventoriedEntity inventoried))
            {
                return;
            }

            InventoryComponent inv = inventoried.Inventory;
            ItemInstanceDto[] subinv = inv.GetSubInvFromItemInstance(e.ItemInstance);

            short slot = inv.GetFirstFreeSlot(subinv, e.ItemInstance);

            if (slot == -1)
            {
                Log.Info("No available slot");
                //Not enough space
                return;
            }

            ItemInstanceDto mergeable = subinv[slot];

            if (mergeable != null)
            {
                mergeable.Amount += e.ItemInstance.Amount;
                e.ItemInstance = mergeable;
            }
            else
            {
                e.ItemInstance.Slot = slot;
                subinv[slot] = e.ItemInstance;
            }

            if (!(inv.Entity is IPlayerEntity player))
            {
                return;
            }

            e.ItemInstance.CharacterId = player.Character.Id;
            await player.ActualizeUiInventorySlot(e.ItemInstance.Type, e.ItemInstance.Slot);
        }
    }
}