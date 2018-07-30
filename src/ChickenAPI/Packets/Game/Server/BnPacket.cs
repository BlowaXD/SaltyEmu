namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("bn")]
    public class BnPacket : PacketBase
    {
        [PacketIndex(0)]
        public byte BnNumber { get; set; }

        [PacketIndex(1)]
        public string Message { get; set; }
    }
}