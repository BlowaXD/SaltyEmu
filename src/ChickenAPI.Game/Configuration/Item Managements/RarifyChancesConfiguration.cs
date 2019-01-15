namespace ChickenAPI.Game.Configuration.Item_Managements
{
    public struct RarifyChancesConfiguration
    {
        public double Raren2 { get; set; }
        public double Raren1 { get; set; }
        public double Rare0 { get; set; }
        public double Rare1 { get; set; }
        public double Rare2 { get; set; }
        public double Rare3 { get; set; }
        public double Rare4 { get; set; }
        public double Rare5 { get; set; }
        public double Rare6 { get; set; }
        public double Rare7 { get; set; }
        public double Rare8 { get; set; }
        public short GoldPrice { get; set; }
        public double ReducedChanceFactor { get; set; }
        public double ReducedPriceFactor { get; set; }
        public int RarifyItemNeededQuantity { get; set; }
        public int RarifyItemNeededVnum { get; set; }
        public int ScrollVnum { get; set; }
    }
}