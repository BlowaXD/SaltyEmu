using ChickenAPI.Game.Portals;
using ChickenAPI.Packets.ServerPackets.Portals;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class PortalPacketExtensions
    {
        public static GpPacket GenerateGpPacket(this PortalComponent portal) => new GpPacket
        {
            PortalId = (int)portal.PortalId,
            IsDisabled = portal.IsDisabled,
            SourceX = portal.SourceX,
            SourceY = portal.SourceY,
            MapId = portal.DestinationMapId,
            PortalType = portal.Type,
        };
    }
}