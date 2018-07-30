using ChickenAPI.Enums.Game.Entity;

namespace ChickenAPI.Packets.Game.Client
{

    [PacketHeader("dir")]
    public class DirectionPacket : PacketBase
    {
        #region Properties

        [PacketIndex(1)]
        public int Option { get; set; }

        [PacketIndex(0)]
        public DirectionType DirectionType { get; set; }

        [PacketIndex(2)]
        public long CharacterId { get; set; }

        #endregion
    }
}