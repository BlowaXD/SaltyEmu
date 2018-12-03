using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Shop
{
    [PacketHeader("s_memo")]
    public class SMemoPacket : PacketBase
    {
        [PacketIndex(0)]
        public SMemoPacketType SMemoPacketType { get; set; }

        [PacketIndex(1)]
        public string Message { get; set; }
    }
}