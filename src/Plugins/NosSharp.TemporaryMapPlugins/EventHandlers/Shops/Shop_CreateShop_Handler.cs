using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Player.Extension;
using ChickenAPI.Game.Shops;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Game.Shops.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Shops
{
    public class Shop_CreateShop_Handler : GenericEventPostProcessorBase<ShopPlayerShopCreateEvent
    >
    {
        protected override async Task Handle(ShopPlayerShopCreateEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            if (player.HasShop)
            {
                return;
            }

            var shop = new PersonalShop(player, player.CurrentMap.GetNextId())
            {
                ShopItems = e.ShopItems,
                ShopName = e.Name
            };
            player.Shop = shop;
            await player.BroadcastExceptSenderAsync(player.GeneratePFlagPacket());
            await player.BroadcastAsync(player.GenerateShopPacket());
            // SHOP OPENED

            // ActualizeSpeed
            player.IsSitting = true;
            await player.ActualizePlayerCondition();
            // sitting
        }
    }
}