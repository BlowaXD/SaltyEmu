using NosSharp.World.Cryptography.Decoder;
using NosSharp.World.Cryptography.Encoder;

namespace NosSharp.World.Cryptography
{
    public class WorldCryptoFactory : IPacketCryptoFactory
    {
        public IEncoder GetEncoder() => new WorldEncoder();

        public IDecoder GetDecoder() => new WorldDecoder();
    }
}