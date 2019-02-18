using ChickenAPI.Game;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Game.Configuration.Item_Managements;

namespace SaltyEmu.BasicPlugin
{
    public class JsonGameConfiguration : IGameConfiguration
    {
        public RateConfiguration Rates { get; set; } = new RateConfiguration
        {
            GlobalFairyXpMultiplier = 1.0f,
            GlobalHeroXpMultiplier = 1.0f,
            GlobalJobXpMultiplier = 1.0f,
            GlobalReputationMultiplier = 1.0f,
            GlobalSpXpMultiplier = 1.0f,
            GoldWinOnDropMultiplier = 1.0f,
            GlobalXpMultiplier = 1.0f,
        };

        public InventoryConfiguration Inventory { get; set; } = new InventoryConfiguration
        {
            MaxItemPerSlot = short.MaxValue
        };

        #region UpgradeConfig

        public RarifyChancesConfiguration RarifyChances { get; set; } = new RarifyChancesConfiguration
        {
            Raren2 = 80,
            Raren1 = 70,
            Rare0 = 60,
            Rare1 = 40,
            Rare2 = 30,
            Rare3 = 15,
            Rare4 = 10,
            Rare5 = 5,
            Rare6 = 3,
            Rare7 = 2,
            Rare8 = 0.5,
            GoldPrice = 500,
            ReducedChanceFactor = 1.1,
            ReducedPriceFactor = 0.5,
            RarifyItemNeededQuantity = 5,
            RarifyItemNeededVnum = 1014,
            ScrollVnum = 1218,
        };

        public UpgradeConfigurationItem UpgradeItem { get; set; } = new UpgradeConfigurationItem
        {
            UpFail = new short[] { 0, 0, 0, 5, 20, 40, 70, 85, 92, 98 },
            UpFix = new short[] { 0, 0, 10, 15, 20, 20, 10, 5, 3, 1 },
            GoldPrice = new int[] { 500, 1500, 3000, 10000, 30000, 80000, 150000, 400000, 700000, 1000000 },
            CellaAmount = new short[] { 20, 50, 80, 120, 160, 220, 280, 380, 480, 600 },
            GemAmount = new short[] { 1, 1, 2, 2, 3, 1, 1, 2, 2, 3 },
            UpFailR8 = new short[] { 50, 40, 60, 50, 60, 70, 75, 77, 83, 89 },
            UpFixR8 = new short[] { 50, 40, 70, 65, 80, 90, 95, 97, 98, 99 },
            GoldPriceR8 = new int[] { 5000, 15000, 30000, 100000, 300000, 800000, 1500000, 4000000, 7000000, 10000000 },
            CellaAmountR8 = new short[] { 40, 100, 160, 240, 320, 440, 560, 760, 960, 1200 },
            GemAmountR8 = new short[] { 2, 2, 4, 4, 6, 2, 2, 4, 4, 6 },
            MaximumUpgrade = 10,
            CellaVnum = 1014,
            GemFullVnum = 1016,
            GemVnum = 1015,
            GoldScrollVnum = 5369,
            NormalScrollVnum = 1218,
            ReducedPriceFactor = 0.5
        };

        public UpgradeConfigurationSp UpgradeSp { get; set; } = new UpgradeConfigurationSp
        {
            GoldPrice = new int[]
            {
                200000, 200000, 200000, 200000, 200000, 500000, 500000, 500000, 500000, 500000, 1000000, 1000000,
                1000000, 1000000, 1000000
            },
            Destroy = new short[] { 0, 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 70 },
            UpFail = new short[] { 20, 25, 30, 40, 50, 60, 65, 70, 75, 80, 90, 93, 95, 97, 99 },
            Feather = new short[] { 3, 5, 8, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 70 },
            FullMoon = new short[] { 1, 3, 5, 7, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30 },
            Soul = new short[] { 2, 4, 6, 8, 10, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 },
            FeatherVnum = 2282,
            FullmoonVnum = 1030,
            GreenSoulVnum = 2283,
            RedSoulVnum = 2284,
            BlueSoulVnum = 2285,
            DragonSkinVnum = 2511,
            DragonBloodVnum = 2512,
            DragonHeartVnum = 2513,
            BlueScrollVnum = 1363,
            RedScrollVnum = 1364
        };

        public SummingConfiguration Summing { get; set; } = new SummingConfiguration
        {
            MaxSum = 6,
            SandVnum = 1027,
            GoldPrice = new int[] { 1500, 3000, 6000, 12000, 24000, 48000 },
            SandAmount = new short[] { 5, 10, 15, 20, 25, 30 },
            UpSucess = new short[] { 100, 100, 85, 70, 50, 20 }
        };

        public PerfectSpConfiguration PerfectSp { get; set; } = new PerfectSpConfiguration
        {
            UpMode = 1,
            GoldPrice = new int[] { 5000, 10000, 20000, 50000, 100000 },
            StonePrice = new short[] { 1, 2, 3, 4, 5 },
            UpSuccess = new short[] { 50, 40, 30, 20, 10 }
        };

        #endregion

        public uint SpMaxDailyPoints { get; set; } = 10000;
        public uint SpMaxAdditionalPoints { get; set; } = 1000000;
        public float ItemPriceOnSellMultiplier { get; set; } = 1.0f;
        public float ItemPriceOnBuyMultiplier { get; set; } = 1.0f;
        public uint MaxGold { get; set; } = 1000000000;
        public string GlobalChatPrefix { get; set; } = "<Total>";
    }
}