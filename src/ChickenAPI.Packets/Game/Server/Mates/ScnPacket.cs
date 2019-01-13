using ChickenAPI.Enums.Game.Mates;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Mates
{
    /// <summary>
    ///     Packets for partners
    /// </summary>
    [PacketHeader("sc_n")]
    public class ScnPacket : PacketBase
    {
        [PacketIndex(0)]
        public long PetId { get; set; }

        [PacketIndex(1)]
        public long NpcMonsterVNum { get; set; }

        [PacketIndex(2)]
        public long TransportId { get; set; }

        [PacketIndex(3)]
        public short Level { get; set; }

        [PacketIndex(4)]
        public short Loyalty { get; set; }

        [PacketIndex(5)]
        public long Experience { get; set; }

        [PacketIndex(6)]
        public ScEquipmentDetails WeaponInstanceDetails { get; set; }

        [PacketIndex(7)]
        public ScEquipmentDetails ArmorInstanceDetails { get; set; }

        [PacketIndex(8)]
        public ScEquipmentDetails GauntletInstanceDetails { get; set; }

        [PacketIndex(9)]
        public ScEquipmentDetails BootsInstanceDetails { get; set; }

        [PacketIndex(10)]
        public short Unknown { get; set; }

        [PacketIndex(11)]
        public short Unknown2 { get; set; }

        [PacketIndex(12)]
        public short Unknown3 { get; set; }

        [PacketIndex(13)]
        public short AttackUpgrade { get; set; }

        [PacketIndex(14)]
        public int MinimumAttack { get; set; }

        [PacketIndex(15)]
        public int MaximumAttack { get; set; }

        [PacketIndex(16)]
        public int Precision { get; set; }

        [PacketIndex(17)]
        public int CriticalRate { get; set; }

        [PacketIndex(18)]
        public int CriticalDamageRate { get; set; }

        [PacketIndex(19)]
        public short DefenceUpgrade { get; set; }

        [PacketIndex(20)]
        public int Defence { get; set; }

        [PacketIndex(21)]
        public int DefenceDodge { get; set; }

        [PacketIndex(22)]
        public int DistanceDefence { get; set; }

        [PacketIndex(23)]
        public int DistanceDodge { get; set; }

        [PacketIndex(24)]
        public int DodgeRate { get; set; }

        [PacketIndex(25)]
        public int ElementRate { get; set; }

        [PacketIndex(26)]
        public int FireResistance { get; set; }

        [PacketIndex(27)]
        public int WaterResistance { get; set; }

        [PacketIndex(28)]
        public int LightResistance { get; set; }

        [PacketIndex(29)]
        public int DarkResistance { get; set; }

        [PacketIndex(30)]
        public int Hp { get; set; }

        [PacketIndex(31)]
        public int HpMax { get; set; }

        [PacketIndex(32)]
        public int Mp { get; set; }

        [PacketIndex(33)]
        public int MpMax { get; set; }

        [PacketIndex(34)]
        public int Unknown21 { get; set; }

        [PacketIndex(35)]
        public int Unknown22 { get; set; }

        /// <summary>
        ///     Spaces should be replaced by "^"
        /// </summary>
        [PacketIndex(36)]
        public string Name { get; set; }

        /// <summary>
        ///     Sp Instance or Skin
        /// </summary>
        [PacketIndex(37)]
        public int MorphId { get; set; }

        /// <summary>
        ///     Sp Instance or Skin
        /// </summary>
        [PacketIndex(38)]
        public bool IsSummonable { get; set; }

        /// <summary>
        ///     Sp Instance or Skin
        /// </summary>
        [PacketIndex(39)]
        public ScSpDetails SpDetails { get; set; }

        /// <summary>
        ///     Sp Instance or Skin
        /// </summary>
        [PacketIndex(40)]
        public ScSkillDetails Skill1Details { get; set; }

        /// <summary>
        ///     Sp Instance or Skin
        /// </summary>
        [PacketIndex(41)]
        public ScSkillDetails Skill2Details { get; set; }

        /// <summary>
        ///     Sp Instance or Skin
        /// </summary>
        [PacketIndex(42)]
        public ScSkillDetails Skill3Details { get; set; }

        [PacketHeader("SUBPACKET_SC_SP")]
        public class ScSpDetails : PacketBase
        {
            [PacketIndex(0)]
            public long ItemId { get; set; }

            [PacketIndex(1, SeparatorBeforeProperty = ".")]
            public byte AgilityPercentage { get; set; }
        }

        [PacketHeader("SUBPACKET_SC_EQ")]
        public class ScEquipmentDetails : PacketBase
        {
            [PacketIndex(0)]
            public long ItemId { get; set; }

            [PacketIndex(1, SeparatorBeforeProperty = ".")]
            public long ItemRare { get; set; }

            [PacketIndex(2, SeparatorBeforeProperty = ".")]
            public long ItemUpgrade { get; set; }
        }

        [PacketHeader("SUBPACKET_SC_SP_SKILL")]
        public class ScSkillDetails : PacketBase
        {
            [PacketIndex(0)]
            public long SkillId { get; set; }

            [PacketIndex(1, SeparatorBeforeProperty = ".")]
            public SpPartnerRank Rank { get; set; }
        }
    }
}

/*
        sc_n
        {PetId}
        {NpcMonsterVNum}
        {MateTransportId}
        {Level}
        {Loyalty}
        {Experience}
        {WeaponDetails (vnum.rare.upgrade) : "-1"} // weapon details
        {ArmorDetails (vnum.rare.upgrade) : "-1"} // armor details
        {GlovesDetails (vnum.0.0) : "-1"} // gaunt details
        {BoostDetails (vnum.0.0) : "-1"} // boots details
        0 // unknown
        0 // unknown 2
        1 // unknown 3
        0 // unknown 4
        142 // unknown 5
        174 // unknown 6
        232 // unknown 7
        4 // unknown 8
        70 // unknown 9
        0 // unknown 10
        73 // unknown 11
        158 // unknown 12
        86 // unknown 13
        158 // unknown 14
        69 // dodge rate
        0 // element rate 16
        0 // unknown 17
        0 // unknown 18
        0 // unknown 19
        0 // unknown 20
        {Hp}
        {MaxHp}
        {Mp}
        {MaxMp}
        0 // unknown 21
        285816 // unknown 22
        {Name.Replace(' ', '^')}
        {-1} // skin
        {(IsSummonable ? 1 : 0)}
        {(SpInstance != null ? $"{SpInstance.ItemVNum}.100" : "-1")}
        {(SkillId.SkillRank)  :) // unknown 23 SpRank1
        -1 // unknown 24 SpRank2
        -1  // unknown 25 SpRank3
 */