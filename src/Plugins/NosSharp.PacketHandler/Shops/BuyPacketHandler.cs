using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Packets.Game.Client.Shops;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Shops
{
    public class BuyPacketHandler : GenericGamePacketHandlerAsync<BuyPacket>
    {
        protected override Task Handle(BuyPacket packet, IPlayerEntity player) =>
            player.EmitEventAsync(new ShopBuyEvent
            {
                Amount = packet.Amount,
                OwnerId = packet.OwnerId,
                Slot = packet.Slot,
                Type = packet.Type
            });
    }
}