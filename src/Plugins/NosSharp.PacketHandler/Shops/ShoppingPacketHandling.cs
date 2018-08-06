using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Shops;
using ChickenAPI.Game.Features.Shops.Args;
using ChickenAPI.Game.Packets.Game.Client;

namespace NosSharp.PacketHandler.Shops
{
    public class ShoppingPacketHandling
    {
        public static void OnShoppingPacketReceived(ShoppingPacket packet, IPlayerEntity player)
        {
            IEnumerable<INpcEntity> npcs = player.EntityManager.GetEntitiesByType<INpcEntity>(EntityType.Npc);

            INpcEntity npc = npcs.FirstOrDefault(s => s.MapNpc.Id == packet.NpcId);
            if (npc == null || !(npc is NpcEntity shopEntity))
            {
                return;
            }

            player.NotifySystem<ShopSystem>(new GetShopInformationEventArgs { Shop = shopEntity.Shop, Type = packet.Type });
        }
    }
}