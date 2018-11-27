using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.UserInterface
{
    [PacketHeader("bsinfo")]
    public class BSInfoPacket : PacketBase
    {
        [PacketIndex(0)]
        public byte Mode { get; set; }

        [PacketIndex(1)]
        public short Title { get; set; }

        [PacketIndex(2)]
        public short Time { get; set; }

        [PacketIndex(3)]
        public short Text { get; set; }
    }
}