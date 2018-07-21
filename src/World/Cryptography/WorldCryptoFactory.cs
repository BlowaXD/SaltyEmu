using World.Cryptography.Decoder;
using World.Cryptography.Encoder;

namespace World.Cryptography
{
    public class WorldCryptoFactory : IPacketCryptoFactory
    {
        public IEncoder GetEncoder() => new WorldEncoder();

        public IDecoder GetDecoder() => new WorldDecoder();
    }
}