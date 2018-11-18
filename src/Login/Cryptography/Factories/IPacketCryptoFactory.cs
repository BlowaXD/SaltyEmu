using Login.Cryptography.Decoder;
using Login.Cryptography.Encoder;

namespace Login.Cryptography.Factories
{
    public interface IPacketCryptoFactory
    {
        IEncoder GetEncoder();
        IDecoder GetDecoder();
    }
}