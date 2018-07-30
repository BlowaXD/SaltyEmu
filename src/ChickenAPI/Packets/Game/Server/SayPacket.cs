using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;

namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("say")]
    public class SayPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public SayColorType Type { get; set; }

        [PacketIndex(3)]
        public string Message { get; set; }

        #endregion
    }
}