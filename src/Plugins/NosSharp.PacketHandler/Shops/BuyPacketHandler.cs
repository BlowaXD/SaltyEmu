using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Packets.ClientPackets.Shops;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Shops
{
    public class BuyPacketHandler : GenericGamePacketHandlerAsync<BuyPacket>
    {
        public BuyPacketHandler(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(BuyPacket packet, IPlayerEntity player)
        {
            await player.EmitEventAsync(new ShopBuyEvent
            {
                Amount = packet.Amount,
                OwnerId = packet.VisualId,
                Slot = packet.Slot,
                Type = packet.VisualType
            });
        }
    }
}