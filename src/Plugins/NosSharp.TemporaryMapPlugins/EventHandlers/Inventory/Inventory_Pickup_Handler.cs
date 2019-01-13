using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Inventory
{
    public class Inventory_Pickup_Handler : GenericEventPostProcessorBase<InventoryPickUpEvent>
    {
        private readonly IItemInstanceDtoFactory _itemFactory;

        public Inventory_Pickup_Handler(IItemInstanceDtoFactory itemFactory)
        {
            _itemFactory = itemFactory;
        }

        protected override async Task Handle(InventoryPickUpEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            ItemInstanceDto[] subinv = player.Inventory.GetSubInvFromItem(e.Drop.Item);
            short slot = player.Inventory.GetFirstFreeSlot(subinv, e.Drop.Item, (short)e.Drop.Quantity);
            if (slot == -1)
            {
                Log.Info("No available slot");
                return;
            }

            ItemInstanceDto itemInstance = e.Drop.ItemInstance ?? _itemFactory.CreateItem(e.Drop.Item, (short)e.Drop.Quantity);
            await player.BroadcastAsync(player.GenerateGetPacket(e.Drop.Id));
            e.Drop.CurrentMap.UnregisterEntity(e.Drop);
            e.Drop.Dispose();

            await player.EmitEventAsync(new InventoryAddItemEvent { ItemInstance = itemInstance });
        }
    }
}