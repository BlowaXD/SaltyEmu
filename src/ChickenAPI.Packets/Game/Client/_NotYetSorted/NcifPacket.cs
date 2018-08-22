using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client._NotYetSorted
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