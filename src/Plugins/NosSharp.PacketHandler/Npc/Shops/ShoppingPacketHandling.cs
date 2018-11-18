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
    public class ShoppingPacketHandling
    {
        public static void OnShoppingPacketReceived(ShoppingPacket packet, IPlayerEntity player)
        {
            IEnumerable<INpcEntity> npcs = player.CurrentMap.GetEntitiesByType<INpcEntity>(VisualType.Npc);

            INpcEntity npc = npcs.FirstOrDefault(s => s.MapNpc.Id == packet.NpcId);
            if (npc == null || !(npc is NpcEntity shopEntity))
            {
                return;
            }

            player.EmitEvent(new GetShopInformationEventArgs { Shop = shopEntity.Shop, Type = packet.Type });
        }
    }
}