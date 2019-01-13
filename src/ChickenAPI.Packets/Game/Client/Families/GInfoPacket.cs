using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Families;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Families
{
    [PacketHeader("ginfo")]
    public class GInfoPacket : PacketBase
    {
        [PacketIndex(0)]
        public string FamilyName { get; set; }

        [PacketIndex(1)]
        public string CharacterName { get; set; }

        /// <summary>
        ///     Todo to confirm
        /// </summary>
        [PacketIndex(2)]
        public GenderType FamilyHeadGenderType { get; set; }

        [PacketIndex(3)]
        public short FamilyLevel { get; set; }

        [PacketIndex(4)]
        public int FamilyXp { get; set; }

        [PacketIndex(5)]
        public uint MaxFamilyXp { get; set; }

        [PacketIndex(6)]
        public ushort MembersCount { get; set; }

        [PacketIndex(7)]
        public ushort MembersCapacity { get; set; }

        [PacketIndex(8)]
        public FamilyAuthority CharacterFamilyAuthority { get; set; }

        [PacketIndex(9)]
        public bool FamilyManagerCanInvit { get; set; }

        [PacketIndex(10)]
        public bool FamilyManagerCanNotice { get; set; }

        [PacketIndex(11)]
        public bool FamilyManagerCanShout { get; set; }

        [PacketIndex(12)]
        public bool FamilyManagerCanGetHistory { get; set; }

        [PacketIndex(13)]
        public FamilyAuthorityType FamilyManagerAuthorityType { get; set; }

        [PacketIndex(14)]
        public bool FamilyMemberCanGetHistory { get; set; }

        [PacketIndex(15)]
        public FamilyAuthorityType FamilyMemberAuthorityType { get; set; }

        /// <summary>
        ///     Should replace ' ' by '^'
        /// </summary>
        [PacketIndex(16)]
        public string FamilyMessage { get; set; }
    }
}