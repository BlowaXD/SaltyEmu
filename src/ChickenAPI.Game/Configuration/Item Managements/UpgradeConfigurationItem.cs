namespace ChickenAPI.Game.Configuration.Item_Managements
{
    public struct UpgradeConfigurationItem
    {
        #region Item

        public short[] UpFail { get; set; }
        public short[] UpFailR8 { get; set; }
        public short[] UpFix { get; set; }
        public short[] UpFixR8 { get; set; }
        public int[] GoldPrice { get; set; }
        public int[] GoldPriceR8 { get; set; }
        public short[] CellaAmount { get; set; }
        public short[] CellaAmountR8 { get; set; }
        public short[] GemAmount { get; set; }
        public short[] GemAmountR8 { get; set; }
        public byte MaximumUpgrade { get; set; }
        public short CellaVnum { get; set; }
        public short GemVnum { get; set; }
        public short GemFullVnum { get; set; }
        public double ReducedPriceFactor { get; set; }
        public short NormalScrollVnum { get; set; }
        public short GoldScrollVnum { get; set; }

        #endregion
    }
}