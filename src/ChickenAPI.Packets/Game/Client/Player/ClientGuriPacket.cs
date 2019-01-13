using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Player
{
    [PacketHeader("guri")]
    public class ClientGuriPacket : PacketBase
    {
        [PacketIndex(0)]
        public int Type { get; set; }

        [PacketIndex(1)]
        public int Argument { get; set; }

        [PacketIndex(2, IsOptional = true)]
        public long? VisualId { get; set; }

        [PacketIndex(3, IsOptional = true)]
        public long? Data { get; set; }

        [PacketIndex(4, IsOptional = true)]
        public long? Value { get; set; }
    }
}