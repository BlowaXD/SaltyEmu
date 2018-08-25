namespace NosSharp.BasicAlgorithm.CharacterAlgorithms.Xp
{
    public class SpLevelBasedAlgorithm : ILevelBasedDataAlgorithm
    {
        public void Initialize()
        {
            // Load SpData
            Data = new long[256];
            Data[0] = 15000;
            Data[19] = 218000;
            for (int i = 1; i < 19; i++)
            {
                Data[i] = Data[i - 1] + 10000;
            }

            for (int i = 20; i < Data.Length; i++)
            {
                Data[i] = Data[i - 1] + 6 * (3 * i * (i + 1) + 1);
            }
        }

        public long[] Data { get; set; }
    }
}