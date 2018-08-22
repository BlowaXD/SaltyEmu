using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Player
{
    [PacketHeader("c_info")]
    public class CInfoPacket : PacketBase
    {
        #region Propertiesf

        [PacketIndex(0)]
        public string Name { get; set; }

        [PacketIndex(1)]
        public string Unknown1 { get; set; }

        [PacketIndex(2)]
        public short GroupId { get; set; }

        [PacketIndex(3)]
        public long FamilyId { get; set; }

        [PacketIndex(4)]
        public string FamilyName { get; set; }

        [PacketIndex(5)]
        public long CharacterId { get; set; }

        [PacketIndex(6)]
        public byte Authority { get; set; }

        [PacketIndex(7)]
        public GenderType Gender { get; set; }

        [PacketIndex(8)]
        public HairStyleType HairStyle { get; set; }

        [PacketIndex(9)]
        public HairColorType HairColor { get; set; }

        [PacketIndex(10)]
        public CharacterClassType Class { get; set; }

        [PacketIndex(11)]
        public byte Icon { get; set; }

        [PacketIndex(12)]
        public short Compliment { get; set; }

        [PacketIndex(13)]
        public short Morph { get; set; }

        [PacketIndex(14)]
        public bool Invisible { get; set; }

        [PacketIndex(15)]
        public byte FamilyLevel { get; set; }

        [PacketIndex(16)]
        public byte SpUpgrade { get; set; }

        [PacketIndex(17)]
        public bool ArenaWinner { get; set; }

        #endregion
    }
}