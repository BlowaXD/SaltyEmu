using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Shops.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers.Shops
{
    public class Shop_Sell_Handler : GenericEventPostProcessorBase<ShopSellEvent>
    {
        public Shop_Sell_Handler(ILogger log) : base(log)
        {
        }

        protected override Task Handle(ShopSellEvent e, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }
}