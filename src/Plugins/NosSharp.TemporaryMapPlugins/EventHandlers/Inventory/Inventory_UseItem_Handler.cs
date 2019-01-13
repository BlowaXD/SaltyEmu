using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage;

namespace SaltyEmu.BasicPlugin.EventHandlers.Inventory
{
    public class Inventory_UseItem_Handler : GenericEventPostProcessorBase<InventoryUseItemEvent>
    {
        private readonly IItemUsageContainer _itemUsageHandler;

        public Inventory_UseItem_Handler(IItemUsageContainer itemUsageHandler)
        {
            _itemUsageHandler = itemUsageHandler;
        }

        protected override Task Handle(InventoryUseItemEvent e, CancellationToken cancellation)
        {
            return Task.Run(() =>
            {
                _itemUsageHandler.UseItem(e.Sender as IPlayerEntity, e);
            }, cancellation);
        }
    }
}