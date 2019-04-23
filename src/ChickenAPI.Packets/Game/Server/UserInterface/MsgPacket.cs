using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.UserInterface
{
    [PacketHeader("msg")]
    public class MsgPacket : PacketBase
    {
        [PacketIndex(0)]
        public MsgPacketType Type { get; set; }

        [PacketIndex(1, SerializeToEnd = true)]
        public string Message { get; set; }
    }
}