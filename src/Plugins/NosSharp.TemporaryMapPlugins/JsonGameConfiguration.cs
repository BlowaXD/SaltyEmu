using ChickenAPI.Game;

namespace SaltyEmu.BasicPlugin
{
    public class JsonGameConfiguration : IGameConfiguration
    {
        public float GlobalXpMultiplier { get; set; } = 1.0f;
        public float GlobalJobXpMultiplier { get; set; } = 1.0f;
        public float GlobalSpXpMultiplier { get; set; } = 1.0f;
        public float GlobalHeroXpMultiplier { get; set; } = 1.0f;
        public float GlobalFairyXpMultiplier { get; set; } = 1.0f;
        public float GoldWinOnDropMultiplier { get; set; } = 1.0f;
        public float GlobalReputationMultiplier { get; set; } = 1.0f;
        public uint SpMaxDailyPoints { get; set; } = 10000;
        public uint SpMaxAdditionalPoints { get; set; } = 1000000;
        public float ItemPriceOnSellMultiplier { get; set; } = 1.0f;
        public float ItemPriceOnBuyMultiplier { get; set; } = 1.0f;
    }
}