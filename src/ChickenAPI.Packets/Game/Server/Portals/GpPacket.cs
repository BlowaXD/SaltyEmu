using ChickenAPI.Enums.Game.Portals;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Portals
{
    /// <summary>
    ///     gp {SourceX} {SourceY} {ServerManager.Instance.GetMapInstance(DestinationMapInstanceId)?.Map.MapId ?? 0} {Type}
    ///     {PortalId} {(IsDisabled ? 1 : 0)}
    /// </summary>
    [PacketHeader("gp")]
    public class GpPacket : PacketBase
    {
        [PacketIndex(0)]
        public short PositionX { get; set; }

        [PacketIndex(1)]
        public short PositionY { get; set; }

        [PacketIndex(2)]
        public long DestinationMapId { get; set; }

        [PacketIndex(3)]
        public PortalType PortalType { get; set; }

        [PacketIndex(4)]
        public long PortalId { get; set; }

        [PacketIndex(5)]
        public bool IsDisabled { get; set; }
    }
}