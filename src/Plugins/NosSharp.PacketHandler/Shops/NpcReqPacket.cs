using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Shops.Args;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Game.Shops.Extensions;
using ChickenAPI.Packets.Game.Client.Shops;
using NLog.LayoutRenderers;

namespace NosSharp.PacketHandler.Npc.Shops
{
    public class NpcReqPacketHandling
    {
        public static void OnNpcReqReceived(ReceivedNpcReqPacket packet, IPlayerEntity player)
        {
            if (packet.VisualType == VisualType.Character)
            {
                IPlayerEntity shop = player.CurrentMap.GetPlayerById(packet.VisualId);
                if (shop == null)
                {
                    return;
                }

                player.EmitEvent(new ShopGetInformationEvent { Shop = shop, Type = 0 });
                return;
            }

            var npc = player.CurrentMap.GetEntity<INpcEntity>(packet.VisualId, VisualType.Npc);

            player.SendPacket(new SentNpcReqPacket
            {
                VisualType = VisualType.Npc,
                VisualId = npc.MapNpc.Id,
                Dialog = npc.MapNpc.Dialog
            });
        }

        public static void BuyPcket(BuyPacket packet, IPlayerEntity player)
        {
            player.EmitEvent(new ShopBuyEvent
            {
                Amount = packet.Amount,
                OwnerId = packet.OwnerId,
                Slot = packet.Slot,
                Type = packet.Type
            });
        }
    }
}