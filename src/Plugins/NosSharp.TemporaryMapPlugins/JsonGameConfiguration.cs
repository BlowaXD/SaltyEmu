using ChickenAPI.Game;

namespace SaltyEmu.BasicPlugin
{
    public class JsonGameConfiguration : IGameConfiguration
    {
        public uint GlobalXpMultiplier { get; set; } = 1;
        public uint GlobalJobXpMultiplier { get; set; } = 1;
        public uint GlobalSpXpMultiplier { get; set; } = 1;
        public uint GlobalHeroXpMultiplier { get; set; } = 1;
        public uint GlobalFairyXpMultiplier { get; set; } = 1;
        public uint GoldWinOnDropMultiplier { get; set; } = 1;
        public uint GlobalReputationMultiplier { get; set; } = 1;
        public uint SpMaxDailyPoints { get; set; } = 10000;
        public uint SpMaxAdditionalPoints { get; set; } = 1000000;
    }
}