using ChickenAPI.Enums.Game.Entity;

namespace ChickenAPI.Packets.Game.Client
{

    [PacketHeader("ncif")]
    public class NcifPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public VisualType Type { get; set; }

        [PacketIndex(1)]
        public long TargetId { get; set; }

        #endregion
    }
}