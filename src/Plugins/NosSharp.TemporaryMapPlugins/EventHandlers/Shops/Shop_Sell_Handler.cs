using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Shops.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers.Shops
{
    public class Shop_Sell_Handler : GenericEventPostProcessorBase<ShopSellEvent>
    {
        protected override async Task Handle(ShopSellEvent e, CancellationToken cancellation)
        {
        }
    }
}