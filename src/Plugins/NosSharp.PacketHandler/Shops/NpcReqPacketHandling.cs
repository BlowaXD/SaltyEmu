using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Packets.ClientPackets.Npcs;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Shop;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Shops
{
    public class NpcReqPacketHandling : GenericGamePacketHandlerAsync<RequestNpcPacket>
    {
        public NpcReqPacketHandling(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(RequestNpcPacket packet, IPlayerEntity player)
        {
            if (packet.Type == VisualType.Player)
            {
                IPlayerEntity shop = player.CurrentMap.GetPlayerById(packet.TargetId);
                if (shop == null)
                {
                    return;
                }

                await player.EmitEventAsync(new ShopGetInformationEvent { Shop = shop, Type = 0 });
                return;
            }

            var npc = player.CurrentMap.GetEntity<INpcEntity>(packet.TargetId, VisualType.Npc);

            if (npc == null)
            {
                return;
            }

            await player.SendPacketAsync(new RequestNpcPacket()
            {
                Type = VisualType.Npc,
                TargetId = npc.MapNpc.Id,
                Data = npc.MapNpc.Dialog
            });
        }
    }
}