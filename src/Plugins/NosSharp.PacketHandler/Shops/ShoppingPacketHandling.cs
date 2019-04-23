using System.Threading.Tasks;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Packets.Old.Game.Client.Shops;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Shops
{
    public class ShoppingPacketHandling : GenericGamePacketHandlerAsync<ShoppingPacket>
    {
        protected override Task Handle(ShoppingPacket packet, IPlayerEntity player)
        {
            var npc = player.CurrentMap.GetEntity<INpcEntity>(packet.NpcId, VisualType.Npc);
            if (npc == null)
            {
                Log.Info("npc null");
                return Task.CompletedTask;
            }

            return player.EmitEventAsync(new ShopGetInformationEvent
            {
                Shop = npc,
                Type = packet.Type
            });
        }
    }
}