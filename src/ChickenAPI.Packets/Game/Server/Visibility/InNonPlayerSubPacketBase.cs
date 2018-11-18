using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Visibility
{
    [PacketHeader("in_non_player_subpacket")]
    public class InNonPlayerSubPacketBase : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public short Dialog { get; set; }

        [PacketIndex(1)]
        public byte Unknown { get; set; }

        #endregion
    }
}