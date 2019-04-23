using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Mates
{
    [PacketHeader("u_pet")]
    public class UpetPacket : PacketBase
    {
        [PacketIndex(0)]
        public long MateTransportId { get; set; }

        [PacketIndex(1)]
        public VisualType TargetType { get; set; }

        [PacketIndex(2)]
        public long TargetId { get; set; }

        [PacketIndex(3)]
        public int Unknown2 { get; set; }
    }
}