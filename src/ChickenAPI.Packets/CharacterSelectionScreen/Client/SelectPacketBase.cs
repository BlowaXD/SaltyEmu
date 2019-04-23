using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.CharacterSelectionScreen.Client
{
    [PacketHeader("select", NeedCharacter = false)]
    public class SelectPacketBase : PacketBase
    {
        [PacketIndex(0)]
        public byte Slot { get; set; }
    }
}