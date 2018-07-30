namespace NosSharp.BasicAlgorithm.CharacterAlgorithms.Xp
{
    public class HeroLevelBasedAlgorithm : ILevelBasedDataAlgorithm
    {
        public void Initialize()
        {
            int index = 1;
            int increment = 118980;
            int increment2 = 9120;
            int increment3 = 360;

            Data = new long[256];
            Data[0] = 949560;
            for (int lvl = 1; lvl < Data.Length; lvl++)
            {
                Data[lvl] = Data[lvl - 1] + increment;
                increment2 += increment3;
                increment = increment + increment2;
                index++;
                if ((index % 10) != 0)
                {
                    continue;
                }

                if (index / 10 < 3)
                {
                    increment3 -= index / 10 * 30;
                }
                else
                {
                    increment3 -= 30;
                }
            }
        }

        public long[] Data { get; set; }
    }
}