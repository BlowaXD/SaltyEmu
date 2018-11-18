using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Shops.Args;
using ChickenAPI.Packets.Game.Client.Shops;

namespace NosSharp.PacketHandler.Npc.Shops
{
    public class NpcReqPacketHandling
    {
        public static void OnShoppingPacketReceived(ReceivedNpcReqPacket packet, IPlayerEntity player)
        {
            IEnumerable<INpcEntity> npcs = player.CurrentMap.GetEntitiesByType<INpcEntity>(VisualType.Npc);

            INpcEntity npc = npcs.FirstOrDefault(s => s.MapNpc.Id == packet.VisualId);
            if (npc == null || !(npc is NpcEntity npcEntity))
            {
                return;
            }

            player.SendPacket(new SentNpcReqPacket
            {
                VisualType = VisualType.Npc,
                VisualId = npc.MapNpc.Id,
                Dialog = npc.MapNpc.Dialog
            });
        }

        public static void BuyPcket(BuyPacket packet, IPlayerEntity player)
        {
            player.EmitEvent(new BuyShopEventArgs
            {
                Amount = packet.Amount,
                OwnerId = packet.OwnerId,
                Slot = packet.Slot,
                Type = packet.Type
            });
        }
    }
}