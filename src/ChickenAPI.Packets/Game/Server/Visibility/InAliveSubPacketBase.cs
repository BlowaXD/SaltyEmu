using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Visibility
{
    [PacketHeader("in_alive_subpacket")]
    public class InAliveSubPacketBase : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public byte HpPercentage { get; set; }

        [PacketIndex(1)]
        public byte MpPercentage { get; set; }

        #endregion
    }
}