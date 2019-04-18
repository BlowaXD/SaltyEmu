using System.Threading.Tasks;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Packets.Game.Client.Shops;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Shops
{
    public class NpcReqPacketHandling : GenericGamePacketHandlerAsync<ReceivedNpcReqPacket>
    {
        protected override async Task Handle(ReceivedNpcReqPacket packet, IPlayerEntity player)
        {
            if (packet.VisualType == VisualType.Player)
            {
                IPlayerEntity shop = player.CurrentMap.GetPlayerById(packet.VisualId);
                if (shop == null)
                {
                    return;
                }

                await player.EmitEventAsync(new ShopGetInformationEvent { Shop = shop, Type = 0 });
                return;
            }

            var npc = player.CurrentMap.GetEntity<INpcEntity>(packet.VisualId, VisualType.Npc);

            await player.SendPacketAsync(new SentNpcReqPacket
            {
                VisualType = VisualType.Npc,
                VisualId = npc.MapNpc.Id,
                Dialog = npc.MapNpc.Dialog
            });
        }
    }
}