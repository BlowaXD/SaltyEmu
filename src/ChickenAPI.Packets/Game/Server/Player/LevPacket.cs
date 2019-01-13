using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Player
{
    /// <summary>
    ///     $"lev {Level} {LevelXp} {(UseSp? SpInstance.SpLevel : JobLevel)} {(UseSp ? SpInstance.XP : JobLevelXp)} {XpLoad()}
    ///     {(UseSp ? SpxpLoad() : JobXpLoad())} {Reput} {GetCp()} {HeroXp} {HeroLevel} {HeroXpLoad()}";
    /// </summary>
    [PacketHeader("lev")]
    public class LevPacket : PacketBase
    {
        [PacketIndex(0)]
        public byte Level { get; set; }

        [PacketIndex(1)]
        public int LevelXp { get; set; }

        [PacketIndex(2)]
        public byte JobLevel { get; set; }

        [PacketIndex(3)]
        public int JobLevelXp { get; set; }

        [PacketIndex(4)]
        public long LevelXpMax { get; set; }

        [PacketIndex(5)]
        public long JobLevelXpMax { get; set; }

        [PacketIndex(6)]
        public long Reputation { get; set; }

        [PacketIndex(7)]
        public int Cp { get; set; }

        [PacketIndex(8)]
        public int HeroLevelXp { get; set; }

        [PacketIndex(9)]
        public byte HeroLevel { get; set; }

        [PacketIndex(10)]
        public int HeroLevelXpMax { get; set; }
    }
}