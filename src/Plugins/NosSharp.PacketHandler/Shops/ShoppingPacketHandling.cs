using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Shops.Args;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Packets.Game.Client.Shops;

namespace NosSharp.PacketHandler.Npc.Shops
{
    public class ShoppingPacketHandling
    {
        public static void OnShoppingPacketReceived(ShoppingPacket packet, IPlayerEntity player)
        {
            var npc = player.CurrentMap.GetEntity<INpcEntity>(packet.NpcId, VisualType.Npc);
            if (npc == null)
            {
                return;
            }

            player.EmitEvent(new ShopGetInformationEvent { Shop = npc, Type = packet.Type });
        }
    }
}