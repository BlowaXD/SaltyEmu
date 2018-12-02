using ChickenAPI.Game;

namespace SaltyEmu.BasicPlugin
{
    public class JsonGameConfiguration : IGameConfiguration
    {
        public uint GlobalXpMultiplier { get; set; }
        public uint GlobalJobXpMultiplier { get; set; }
        public uint GlobalSpXpMultiplier { get; set; }
        public uint GlobalHeroXpMultiplier { get; set; }
        public uint GlobalFairyXpMultiplier { get; set; }
        public uint GoldWinOnDropMultiplier { get; set; }
        public uint GlobalReputationMultiplier { get; set; }
    }
}