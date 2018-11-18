using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Player
{
    [PacketHeader("guri")]
    public class ClientGuriPacket
    {
        [PacketIndex(0)]
        public int Type { get; set; }

        [PacketIndex(1)]
        public int Argument { get; set; }

        [PacketIndex(2)]
        public long? VisualId { get; set; }

        [PacketIndex(3)]
        public int Data { get; set; }

        [PacketIndex(4, true)]
        public string Value { get; set; }
    }
}