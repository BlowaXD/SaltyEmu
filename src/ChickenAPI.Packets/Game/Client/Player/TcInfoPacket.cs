using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Player
{
    [PacketHeader("tc_info")]
    public class TcInfoPacket : PacketBase
    {
        // return $"tc_info {Level} {Name} {fairy?.Item.Element ?? 0} {ElementRate + (Buff.Any(s => s.Card.CardId == 131) ? 30 : 0)}
        // {(byte)Class} {(byte)Gender} {(Family != null ? $"{Family.FamilyId}
        // {Family.Name}({Language.Instance.GetMessageFromKey(FamilyCharacter?.Authority.ToString()?.ToUpper() ?? "")})" : "-1 -")}
        // {GetReputationIco()} {GetDignityIco()} {(weapon != null ? 1 : 0)} {weapon?.Rare ?? 0} {weapon?.Upgrade ?? 0} {(weapon2 != null ? 1 : 0)}
        // {weapon2?.Rare ?? 0} {weapon2?.Upgrade ?? 0} {(armor != null ? 1 : 0)} {armor?.Rare ?? 0} {armor?.Upgrade ?? 0} {Act4Kill} {Act4Dead} {Reputation}
        // 0 0 0 {(UseSp ? Morph : 0)} {TalentWin} {TalentLose} {TalentSurrender} 0 {MasterPoints} {Compliment} {Act4Points} {(isPvpPrimary ? 1 : 0)}
        // {(isPvpSecondary ? 1 : 0)} {(isPvpArmor ? 1 : 0)} {HeroLevel}
        // {(string.IsNullOrEmpty(Biography) ? Language.Instance.GetMessageFromKey("NO_PREZ_MESSAGE") : Biography)}";
        [PacketIndex(0)]
        public long Level { get; set; }

        [PacketIndex(1)]
        public string Name { get; set; }

        [PacketIndex(2)]
        public ElementType Element { get; set; }

        [PacketIndex(3)]
        public long ElementRate { get; set; }

        [PacketIndex(4)]
        public CharacterClassType Class { get; set; }

        [PacketIndex(5)]
        public GenderType Gender { get; set; }

        [PacketIndex(6)]
        public string Family { get; set; }

        [PacketIndex(7)]
        public CharacterRep ReputationIco { get; set; }

        [PacketIndex(8)]
        public CharacterDignity DignityIco { get; set; }

        [PacketIndex(9)]
        public int HaveWeapon { get; set; }

        [PacketIndex(10)]
        public long WeaponRare { get; set; }

        [PacketIndex(11)]
        public long WeaponUpgrade { get; set; }

        [PacketIndex(12)]
        public int HaveSecondary { get; set; }

        [PacketIndex(13)]
        public long SecondaryRare { get; set; }

        [PacketIndex(14)]
        public long SecondaryUpgrade { get; set; }

        [PacketIndex(15)]
        public int HaveArmor { get; set; }

        [PacketIndex(16)]
        public long ArmorRare { get; set; }

        [PacketIndex(17)]
        public long ArmorUpgrade { get; set; }

        [PacketIndex(18)]
        public long Act4Kill { get; set; }

        [PacketIndex(19)]
        public long Act4Dead { get; set; }

        [PacketIndex(20)]
        public long Reputation { get; set; }

        [PacketIndex(21)]
        public long Unknow { get; set; } = 0; // Always 0

        [PacketIndex(22)]
        public long Unknow2 { get; set; } = 0; // Always 0

        [PacketIndex(23)]
        public long Unknow3 { get; set; } = 0; // Always 0

        [PacketIndex(24)]
        public long Morph { get; set; }

        [PacketIndex(25)]
        public long TalentWin { get; set; }

        [PacketIndex(26)]
        public long TalentLose { get; set; }

        [PacketIndex(27)]
        public long TalentSurrender { get; set; }

        [PacketIndex(28)]
        public long Unknow4 { get; set; } = 0; // always 0

        [PacketIndex(29)]
        public long MasterPoints { get; set; }

        [PacketIndex(30)]
        public long Compliments { get; set; }

        [PacketIndex(31)]
        public long Act4Points { get; set; }

        [PacketIndex(32)]
        public bool isPvpPrimary { get; set; }

        [PacketIndex(33)]
        public bool isPvpSecondary { get; set; }

        [PacketIndex(34)]
        public bool isPvpArmor { get; set; }

        [PacketIndex(35)]
        public long HeroLevel { get; set; }

        [PacketIndex(36)]
        public string Biography { get; set; }
    }
}