using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Chat
{
    [PacketHeader("say")]
    public class ClientSayPacket : PacketBase
    {
        [PacketIndex(0, SerializeToEnd = true)]
        public string Message { get; set; }
    }
}