using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Miniland
{
    [PacketHeader("addobj")]
    public class AddobjPacket : PacketBase
    {
        [PacketIndex(0)]
        public short Slot { get; set; }

        [PacketIndex(1)]
        public short MapX { get; set; }

        [PacketIndex(2)]
        public short MapY { get; set; }
    }
}