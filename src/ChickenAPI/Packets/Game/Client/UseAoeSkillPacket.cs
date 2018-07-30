namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("u_as")]
    public class UseAoeSkillPacket : PacketBase
    {
        [PacketIndex(0)]
        public long CastId { get; set; }

        [PacketIndex(1)]
        public short MapX { get; set; }

        [PacketIndex(2)]
        public short MapY { get; set; }
    }
}
