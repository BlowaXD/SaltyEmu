using System;
using System.Security.Cryptography;

namespace ChickenAPI.Core.Maths
{
    public class RandomGenerator : IRandomGenerator
    {
        private RNGCryptoServiceProvider RngProvider { get; }

        public RandomGenerator()
        {
            RngProvider = new RNGCryptoServiceProvider();
        }

        public int Next(int min, int max)
        {
            return (int)Math.Floor(min + ((double)max - min) * Next());
        }

        public int Next(int max)
        {
            return Next(0, max);
        }

        public double Next()
        {
            var bytes = new byte[sizeof(uint)];
            GetBytes(bytes);

            var rnd = BitConverter.ToUInt32(bytes, 0);

            return rnd / (uint.MaxValue + 1.0);
        }

        private void GetBytes(byte[] data)
        {
            RngProvider.GetBytes(data);
        }
    }
}
