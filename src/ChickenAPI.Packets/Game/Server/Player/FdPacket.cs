using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Player
{
    [PacketHeader("fd")]
    public class FdPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public long Reput { get; set; }

        [PacketIndex(1)]
        public int ReputIcon { get; set; }

        [PacketIndex(2)]
        public int Dignity { get; set; }

        [PacketIndex(3)]
        public int DignityIcon { get; set; }

        #endregion
    }
}