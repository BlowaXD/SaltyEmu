using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("c_info")]
    public class CInfoPacketBase : PacketBase
    {
        public CInfoPacketBase(IPlayerEntity entity)
        {
            var character = entity.GetComponent<CharacterComponent>();
            var family = entity.GetComponent<FamilyComponent>();

            Name = entity.GetComponent<NameComponent>().Name;
            Unknown1 = "-"; //TODO: Find signification
            GroupId = -1; //TODO: Find signification 
            FamilyId = family.FamilyId == 0 ? -1 : family.FamilyId;
            FamilyName = family.FamilyName;
            CharacterId = character.Id;
            Authority = character.Authority > AuthorityType.GameMaster ? (byte)2 : (byte)0;
            Gender = character.Gender;
            HairStyle = character.HairStyle;
            HairColor = character.HairColor;
            Class = character.Class;
            Icon = character.ReputIcon;
            Compliment = character.Compliment;
            Morph = character.Morph;
            Invisible = !entity.GetComponent<VisibilityComponent>().IsVisible;
            FamilyLevel = family.FamilyLevel;
            SpUpgrade = entity.GetComponent<SpecialistComponent>().Upgrade;
            ArenaWinner = character.ArenaWinner;
        }

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
        public ReputationIconType Icon { get; set; }

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