using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client._NotYetSorted
{
    [PacketHeader("say")]
    public class ClientSayPacket : PacketBase
    {
        [PacketIndex(0, SerializeToEnd = true)]
        public string Message { get; set; }
    }
}