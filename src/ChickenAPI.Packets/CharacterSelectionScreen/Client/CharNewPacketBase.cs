using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.CharacterSelectionScreen.Client
{
    [PacketHeader("Char_NEW", false)]
    public class CharNewPacketBase : PacketBase
    {
        [PacketIndex(0)]
        public string Name { get; set; }

        [PacketIndex(1)]
        public byte Slot { get; set; }

        [PacketIndex(2)]
        public GenderType Gender { get; set; }

        [PacketIndex(3)]
        public HairStyleType HairStyle { get; set; }

        [PacketIndex(4)]
        public HairColorType HairColor { get; set; }
    }
}