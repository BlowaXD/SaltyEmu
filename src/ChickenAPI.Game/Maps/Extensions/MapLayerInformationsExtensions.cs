using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Game.Visibility;
using ChickenAPI.Packets.Game.Server.Portals;
using ChickenAPI.Packets.Game.Server._NotYetSorted;

namespace ChickenAPI.Game.Maps.Extensions
{
    public static class MapLayerInformationsExtensions
    {
        public static IEnumerable<InPacketBase> GetEntitiesPackets(this IMapLayer layer)
        {
            return layer.Entities.Where(s => s is IVisibleEntity).Select(p => ((IVisibleEntity)p).GenerateInPacket());
        }

        public static IEnumerable<GpPacket> GetPortalsPackets(this IMapLayer map)
        {
            return map.GetEntitiesByType<IPortalEntity>(VisualType.Portal).Select(s => s.Portal.GenerateGpPacket());
        }
    }
}