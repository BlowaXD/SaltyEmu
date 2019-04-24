using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Shop;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Shops
{
    public class NpcReqPacketHandling : GenericGamePacketHandlerAsync<NpcReqPacket>
    {
        public NpcReqPacketHandling(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(NpcReqPacket packet, IPlayerEntity player)
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

            if (npc == null)
            {
                return;
            }

            await player.SendPacketAsync(new NpcReqPacket()
            {
                VisualType = VisualType.Npc,
                VisualId = npc.MapNpc.Id,
                Dialog = npc.MapNpc.Dialog
            });
        }
    }
}