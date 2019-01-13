using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Game.Shops.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Shops
{
    public class Shop_GetInformation_Handler : GenericEventPostProcessorBase<ShopGetInformationEvent>
    {
        protected override async Task Handle(ShopGetInformationEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            switch (e.Shop)
            {
                case INpcEntity npc:
                    await player.SendPacketAsync(player.GenerateNInvPacket(npc.Shop, e.Type));
                    return;
                case IPlayerEntity playerShop:
                    await player.SendPacketAsync(player.GenerateNInvPacket(playerShop.Shop));
                    return;
            }
        }
    }
}