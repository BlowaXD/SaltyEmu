using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.EventHandlers.Inventory
{
    public class Inventory_DestroyItem_Handler : GenericEventPostProcessorBase<InventoryDestroyItemEvent>
    {
        public Inventory_DestroyItem_Handler(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(InventoryDestroyItemEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            InventoryComponent inv = player.Inventory;

            ItemInstanceDto[] subinv = inv.GetSubInvFromItemInstance(e.ItemInstance);

            subinv[e.ItemInstance.Slot] = null;
            await player.ActualizeUiInventorySlot(e.ItemInstance.Type, e.ItemInstance.Slot);
            await player.SendChatMessageAsync("ITEM_DESTROYED", SayColorType.Yellow);
        }
    }
}