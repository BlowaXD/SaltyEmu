namespace ChickenAPI.Packets.CharacterScreen.Client
{
    [PacketHeader("Char_DEL", false)]
    public class CharacterDeletePacketBase : PacketBase
    {
        [PacketIndex(0)]
        public byte Slot { get; set; }

        [PacketIndex(1)]
        public string Password { get; set; }
    }
}