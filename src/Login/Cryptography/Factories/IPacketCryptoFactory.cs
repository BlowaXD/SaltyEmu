using LoginServer.Cryptography.Decoder;
using LoginServer.Cryptography.Encoder;

namespace LoginServer.Cryptography.Factories
{
    public interface IPacketCryptoFactory
    {
        IEncoder GetEncoder();
        IDecoder GetDecoder();
    }
}