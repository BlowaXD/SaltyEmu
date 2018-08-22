namespace ChickenAPI.Game.Packets.CharacterScreen.Client
{
    [PacketHeader("select", NeedCharacter = false)]
    public class SelectPacketBase : PacketBase
    {
        [PacketIndex(0)]
        public byte Slot { get; set; }
    }
}