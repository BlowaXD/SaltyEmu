using WingsEmu.World.Cryptography.Decoder;
using WingsEmu.World.Cryptography.Encoder;

namespace WingsEmu.World.Cryptography
{
    public interface IPacketCryptoFactory
    {
        IEncoder GetEncoder();
        IDecoder GetDecoder();
    }
}