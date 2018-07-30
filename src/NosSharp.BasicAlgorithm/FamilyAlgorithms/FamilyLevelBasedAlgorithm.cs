namespace NosSharp.BasicAlgorithm.FamilyAlgorithms
{
    public class FamilyLevelBasedAlgorithm : ILevelBasedDataAlgorithm
    {
        public void Initialize()
        {
            Data = new long[]
            {
                100000, 250000, 370000, 560000, 840000,
                1260000, 1900000, 2850000, 3570000, 3830000,
                4150000, 4750000, 5500000, 6500000, 7000000,
                8500000, 9500000, 10000000, 17000000, 999999999
            };
        }

        public long[] Data { get; set; }
    }
}