namespace ChickenAPI.Packets.CharacterScreen.Server
{
    [PacketHeader("info")]
    public class InfoPacketBase : PacketBase
    {
        [PacketIndex(0)]
        public string Message { get; set; }
    }
}