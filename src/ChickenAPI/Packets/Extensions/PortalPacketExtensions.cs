using ChickenAPI.Game.Features.Portals;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Packets.Extensions
{
    public static class PortalPacketExtensions
    {
        public static GpPacket GenerateGpPacket(this PortalComponent portal)
        {
            return new GpPacket
            {
                PositionX = portal.SourceX,
                PositionY = portal.SourceY,
                DestinationMapId = portal.DestinationMapId,
                PortalType = portal.Type,
                PortalId = portal.PortalId,
                IsDisabled = portal.IsDisabled,
            };
        }
    }
}