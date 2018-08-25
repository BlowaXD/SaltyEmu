namespace NosSharp.BasicAlgorithm.CharacterAlgorithms.Xp
{
    public class JobLevelBasedAlgorithm : ILevelBasedDataAlgorithm
    {
        public long[] FirstJobXpData { get; set; }

        public void Initialize()
        {
            // Load JobData
            FirstJobXpData = new long[21];
            Data = new long[256];
            FirstJobXpData[0] = 2200;
            Data[0] = 17600;
            for (int i = 1; i < FirstJobXpData.Length; i++)
            {
                FirstJobXpData[i] = FirstJobXpData[i - 1] + 700;
            }

            for (int i = 1; i < Data.Length; i++)
            {
                int var2 = 400;
                if (i > 3)
                {
                    var2 = 4500;
                }

                if (i > 40)
                {
                    var2 = 15000;
                }

                Data[i] = Data[i - 1] + var2;
            }
        }

        public long[] Data { get; set; }
    }
}