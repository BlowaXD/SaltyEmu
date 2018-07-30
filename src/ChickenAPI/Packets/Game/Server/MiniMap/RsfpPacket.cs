namespace ChickenAPI.Packets.Game.Server.MiniMap
{
    [PacketHeader("rsfp")]
    public class RsfpPacket : PacketBase
    {
        [PacketIndex(0)]
        public long MapX { get; set; } = 0;

        [PacketIndex(1)]
        public long MapY { get; set; } = -1;
    }
}