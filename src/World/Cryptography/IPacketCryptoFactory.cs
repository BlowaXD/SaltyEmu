using World.Cryptography.Decoder;
using World.Cryptography.Encoder;

namespace World.Cryptography
{
    public interface IPacketCryptoFactory
    {
        IEncoder GetEncoder();
        IDecoder GetDecoder();
    }
}