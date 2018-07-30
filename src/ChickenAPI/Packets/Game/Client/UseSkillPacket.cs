using ChickenAPI.Enums.Game.Entity;

namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("u_s")]
    public class UseSkillPacket : PacketBase
    {
        [PacketIndex(0)]
        public long CastId { get; set; }

        [PacketIndex(1)]
        public VisualType VisualType { get; set; }

        [PacketIndex(2)]
        public int MapMonsterId { get; set; }

        [PacketIndex(3, IsOptional = true)]
        public short MapX { get; set; }

        [PacketIndex(4, IsOptional = true)]
        public short MapY { get; set; }
    }
}
