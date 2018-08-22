using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Specialists
{
    [PacketHeader("sp")]
    public class SpPacket : PacketBase
    {
        [PacketIndex(0)]
        public int AdditionalPoints { get; set; }

        [PacketIndex(1)]
        public int MaxAdditionalPoints { get; set; }

        [PacketIndex(2)]
        public int Points { get; set; }

        [PacketIndex(3)]
        public int MaxPoints { get; set; }
    }
}