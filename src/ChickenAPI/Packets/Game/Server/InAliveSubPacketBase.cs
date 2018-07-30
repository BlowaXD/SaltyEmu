namespace ChickenAPI.Packets.Game.Server
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
