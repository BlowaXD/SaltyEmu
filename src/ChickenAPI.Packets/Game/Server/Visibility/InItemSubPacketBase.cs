using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Visibility
{
    [PacketHeader("in_item_subpacket")]
    public class InItemSubPacketBase : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public short Unknown { get; set; }

        [PacketIndex(1)]
        public short Unknown1 { get; set; }

        [PacketIndex(2)]
        public short Unknown2 { get; set; }

        #endregion
    }
}