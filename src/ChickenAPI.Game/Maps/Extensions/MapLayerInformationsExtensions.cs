using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Shops.Extensions;
using ChickenAPI.Game.Visibility;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Packets.Game.Client.Shops;
using ChickenAPI.Packets.Game.Server.Inventory;
using ChickenAPI.Packets.Game.Server.Portals;
using ChickenAPI.Packets.Game.Server.Visibility;

namespace ChickenAPI.Game.Maps.Extensions
{
    public static class MapLayerInformationsExtensions
    {
        public static IEnumerable<ShopPacket> GetShopsPackets(this IMapLayer layer)
        {
            IEnumerable<ShopPacket> tmp = layer.GetEntitiesByType<INpcEntity>(VisualType.Npc).Where(s => s.HasShop).Select(s => s.GenerateShopPacket());
            IEnumerable<ShopPacket> players = layer.GetEntitiesByType<IPlayerEntity>(VisualType.Character).Where(s => s.HasShop).Select(s => s.GenerateShopPacket());
            return tmp.Union(players);
        }

        public static IEnumerable<PairyPacket> GetPairyPackets(this IMapLayer layer, IPlayerEntity player)
        {
            return layer.GetEntitiesByType<IPlayerEntity>(VisualType.Character).Select(s => s == player ? null : s.GeneratePairyPacket());
        }

        public static IEnumerable<InPacket> GetEntitiesPackets(this IMapLayer layer)
        {
            return layer.Entities.Where(s => s is IVisibleEntity).Select(p => ((IVisibleEntity)p).GenerateInPacket());
        }

        public static IEnumerable<GpPacket> GetPortalsPackets(this IMapLayer map)
        {
            return map.GetEntitiesByType<IPortalEntity>(VisualType.Portal)?.Select(s => s.Portal.GenerateGpPacket());
        }
    }
}