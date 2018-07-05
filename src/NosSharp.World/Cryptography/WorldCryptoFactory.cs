using WingsEmu.World.Cryptography.Decoder;
using WingsEmu.World.Cryptography.Encoder;

namespace WingsEmu.World.Cryptography
{
    public class WorldCryptoFactory : IPacketCryptoFactory
    {
        public IEncoder GetEncoder() => new WorldEncoder();

        public IDecoder GetDecoder() => new WorldDecoder();
    }
}