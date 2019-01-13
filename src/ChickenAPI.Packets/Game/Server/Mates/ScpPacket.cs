using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Mates
{
    /// <summary>
    ///     Packets for pets
    /// </summary>
    [PacketHeader("sc_p")]
    public class ScpPacket : PacketBase
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
        public long Unknow1 { get; set; }

        [PacketIndex(7)]
        public long AttackUpgrade { get; set; }

        [PacketIndex(8)]
        public long DamageMinimum { get; set; }

        [PacketIndex(9)]
        public long DamageMaximum { get; set; }

        [PacketIndex(10)]
        public long Concentrate { get; set; }

        [PacketIndex(11)]
        public long CriticalChance { get; set; }

        [PacketIndex(12)]
        public long CriticalRate { get; set; }

        [PacketIndex(13)]
        public long DefenceUpgrade { get; set; }

        [PacketIndex(14)]
        public long CloseDefence { get; set; }

        [PacketIndex(15)]
        public long DefenceDodge { get; set; }

        [PacketIndex(16)]
        public long DistanceDefence { get; set; }

        [PacketIndex(17)]
        public long DistanceDefenceDodge { get; set; }

        [PacketIndex(18)]
        public long MagicDefence { get; set; }

        [PacketIndex(19)]
        public long Element { get; set; }

        [PacketIndex(20)]
        public long FireResistance { get; set; }

        [PacketIndex(21)]
        public long WaterResistance { get; set; }

        [PacketIndex(22)]
        public long LightResistance { get; set; }

        [PacketIndex(23)]
        public long DarkResistance { get; set; }

        [PacketIndex(24)]
        public long Hp { get; set; }

        [PacketIndex(25)]
        public long MaxHp { get; set; }

        [PacketIndex(26)]
        public long Mp { get; set; }

        [PacketIndex(27)]
        public long MaxMp { get; set; }

        [PacketIndex(28)]
        public bool IsTeamMember { get; set; }

        [PacketIndex(29)]
        public long XpLoad { get; set; }

        [PacketIndex(30)]
        public bool CanPickUp { get; set; }

        [PacketIndex(31)]
        public string Name { get; set; }

        [PacketIndex(32)]
        public bool IsSummonable { get; set; }
    }
}