using System;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Xp
{
    public class LevelBasedAlgorithm : ILevelBasedDataAlgorithm
    {
        public void Initialize()
        {
            Data = new long[256];
            double[] v = new double[256];
            double var = 1;
            v[0] = 540;
            v[1] = 960;
            Data[0] = 300;
            for (int i = 2; i < v.Length; i++)
            {
                v[i] = v[i - 1] + 420 + 120 * (i - 1);
            }

            for (int i = 1; i < Data.Length; i++)
            {
                if (i < 79)
                {
                    switch (i)
                    {
                        case 14:
                            var = 6 / 3d;
                            break;
                        case 39:
                            var = 19 / 3d;
                            break;
                        case 59:
                            var = 70 / 3d;
                            break;
                    }

                    Data[i] = Convert.ToInt64(Data[i - 1] + var * v[i - 1]);
                }

                if (i < 79)
                {
                    continue;
                }

                switch (i)
                {
                    case 79:
                        var = 5000;
                        break;
                    case 82:
                        var = 9000;
                        break;
                    case 84:
                        var = 13000;
                        break;
                }

                Data[i] = Convert.ToInt64(Data[i - 1] + var * (i + 2) * (i + 2));
            }
        }

        public long[] Data { get; set; }
    }
}