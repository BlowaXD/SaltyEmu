using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.UserInterface
{
    [PacketHeader("qna")]
    public class QnaPacket : PacketBase
    {
        [PacketIndex(0, IsReturnPacket = true)]
        public PacketBase AcceptPacket { get; set; }

        [PacketIndex(1, SerializeToEnd = true)]
        public string Question { get; set; }
    }
}