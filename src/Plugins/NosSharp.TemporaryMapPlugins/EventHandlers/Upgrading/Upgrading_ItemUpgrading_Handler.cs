using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.ItemUpgrade;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers.Handling;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Upgrading_ItemUpgrading_Handler : GenericEventPostProcessorBase<ItemUpgradeEvent>
    {
        private readonly IItemUpgradeHandler _itemUpgradeHandler;


        public Upgrading_ItemUpgrading_Handler(ILogger log, IItemUpgradeHandler itemUpgradeHandler) : base(log) => _itemUpgradeHandler = itemUpgradeHandler;

        protected override Task Handle(ItemUpgradeEvent e, CancellationToken cancellation)
        {
            return Task.Run(() => _itemUpgradeHandler.Execute(e.Sender as IPlayerEntity, e), cancellation);
        }
    }
}