using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("guri")]
    public class ServerGuriPacket : PacketBase
    {
        [PacketIndex(0)]
        public int Type { get; set; }

        [PacketIndex(1)]
        public int Argument { get; set; }

        [PacketIndex(2)]
        public long VisualId { get; set; }

        [PacketIndex(3, IsOptional = true)]
        public long? Value { get; set; }

        [PacketIndex(4, IsOptional = true)]
        public int? Data { get; set; }
    }
}
