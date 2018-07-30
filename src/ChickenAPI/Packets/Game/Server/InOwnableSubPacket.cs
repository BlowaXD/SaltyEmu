namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("in_ownable_subpacket")]
    public class InOwnableSubPacketBase : PacketBase
    {
        #region Properties
        [PacketIndex(0)]
        public short? Unknown { get; set; }

        [PacketIndex(1)]
        public long Owner { get; set; }

        #endregion
    }
}
