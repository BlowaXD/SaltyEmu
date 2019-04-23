using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Shop
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