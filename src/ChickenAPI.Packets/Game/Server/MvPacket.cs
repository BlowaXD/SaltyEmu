using ChickenAPI.Enums.Game.Entity;

namespace ChickenAPI.Game.Packets.Game.Server
{
    [PacketHeader("mv")]
    public class MvPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public short MapX { get; set; }

        [PacketIndex(3)]
        public short MapY { get; set; }

        [PacketIndex(4)]
        public short Speed { get; set; }
    }
}