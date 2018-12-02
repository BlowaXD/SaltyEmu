using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Attributes;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace ChickenAPI.Packets.Game.Server.Visibility
{
    [PacketHeader("in_character_subpacket")]
    public class InCharacterSubPacketBase : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public CharacterNameAppearance NameAppearance { get; set; }

        [PacketIndex(1)]
        public GenderType Gender { get; set; }

        [PacketIndex(2)]
        public HairStyleType HairStyle { get; set; }

        [PacketIndex(3)]
        public HairColorType HairColor { get; set; }

        [PacketIndex(4)]
        public CharacterClassType Class { get; set; }

        [PacketIndex(5, IsOptional = true)]
        public InventoryWearSubPacket Equipment { get; set; }

        [PacketIndex(6)]
        public byte HpPercentage { get; set; }

        [PacketIndex(7)]
        public byte MpPercentage { get; set; }

        [PacketIndex(8)]
        public bool IsSitting { get; set; }

        [PacketIndex(9)]
        public long GroupId { get; set; }

        [PacketIndex(10)]
        public byte FairyId { get; set; }

        [PacketIndex(11)]
        public byte FairyElement { get; set; }

        [PacketIndex(12)]
        public byte IsBoostedFairy { get; set; }

        [PacketIndex(13)]
        public short FairyMorph { get; set; }

        [PacketIndex(14)]
        public byte EntryType { get; set; }

        [PacketIndex(15)]
        public long Morph { get; set; }

        [PacketIndex(16)]
        public string WeaponRareAndUpgradeInfo { get; set; }

        [PacketIndex(17)]
        public string ArmorRareAndUpgradeInfo { get; set; }

        [PacketIndex(18)]
        public long FamilyId { get; set; }

        [PacketIndex(19)]
        public string FamilyName { get; set; }

        [PacketIndex(20)]
        public CharacterRep ReputationIcon { get; set; }

        [PacketIndex(21)]
        public bool Invisible { get; set; }

        [PacketIndex(22)]
        public long SpUpgrade { get; set; }

        [PacketIndex(23)]
        public FactionType Faction { get; set; }

        [PacketIndex(24)]
        public long SpDesign { get; set; }

        [PacketIndex(25)]
        public byte Level { get; set; }

        [PacketIndex(26)]
        public byte FamilyLevel { get; set; }

        [PacketIndex(27)]
        public bool ArenaWinner { get; set; }

        [PacketIndex(28)]
        public int Compliment { get; set; }

        [PacketIndex(29)]
        public short Size { get; set; }

        [PacketIndex(30)]
        public short HeroLevel { get; set; }

        #endregion
    }
}