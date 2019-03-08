using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Player
{
    [PacketHeader("qset")]
    public class QsetPacketReceive : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public short Q1 { get; set; }

        [PacketIndex(1)]
        public short Q2 { get; set; }

        [PacketIndex(2)]
        public string Data { get; set; }

        #endregion
    }
}