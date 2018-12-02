using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Shops
{
    [PacketHeader("s_memo")]
    public class SMemoPacket : PacketBase
    {
        [PacketIndex(0)]
        public short sMemoPacketType { get; set; }

        [PacketIndex(1)]
        public string Message { get; set; }
    }
}