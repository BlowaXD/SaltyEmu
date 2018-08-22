using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("eq")]
    public class EqPacket : PacketBase
    {
        [PacketIndex(0)]
        public long CharacterId { get; set; }

        [PacketIndex(1)]
        public byte VisualType { get; set; }

        [PacketIndex(2)]
        public GenderType GenderType { get; set; }

        [PacketIndex(3)]
        public HairStyleType HairStyleType { get; set; }

        [PacketIndex(4)]
        public HairColorType HairColorType { get; set; }

        [PacketIndex(5)]
        public CharacterClassType CharacterClassType { get; set; }

        [PacketIndex(6)]
        public EqListInfo EqList { get; set; }

        [PacketIndex(7, RemoveSeparator = true)]
        public EqRareInfo EqInfo { get; set; }
    }
}