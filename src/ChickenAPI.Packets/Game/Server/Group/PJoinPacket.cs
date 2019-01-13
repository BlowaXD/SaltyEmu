using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Group
{
    [PacketHeader("pjoin")]
    public class PJoinPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public PJoinPacketType RequestType { get; set; }

        [PacketIndex(1)]
        public long CharacterId { get; set; }

        #endregion
    }
}