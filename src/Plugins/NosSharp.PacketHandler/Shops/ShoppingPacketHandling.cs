using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Packets.ClientPackets.Shops;
using ChickenAPI.Packets.Enumerations;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Shops
{
    public class ShoppingPacketHandling : GenericGamePacketHandlerAsync<ShoppingPacket>
    {
        public ShoppingPacketHandling(ILogger log) : base(log)
        {
        }

        protected override Task Handle(ShoppingPacket packet, IPlayerEntity player)
        {
            var npc = player.CurrentMap.GetEntity<INpcEntity>(packet.VisualId, VisualType.Npc);
            if (npc == null)
            {
                Log.Info("npc null");
                return Task.CompletedTask;
            }

            return player.EmitEventAsync(new ShopGetInformationEvent
            {
                Shop = npc,
                Type = (byte)packet.VisualType
            });
        }
    }
}