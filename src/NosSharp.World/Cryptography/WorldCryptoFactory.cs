using NosSharp.World.Cryptography.Decoder;
using NosSharp.World.Cryptography.Encoder;

namespace NosSharp.World.Cryptography
{
    public class WorldCryptoFactory : IPacketCryptoFactory
    {
        public IEncoder GetEncoder()
        {
            return new WorldEncoder();
        }

        public IDecoder GetDecoder()
        {
            return new WorldDecoder();
        }
    }
}