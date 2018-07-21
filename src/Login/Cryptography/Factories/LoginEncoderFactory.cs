using LoginServer.Cryptography.Decoder;
using LoginServer.Cryptography.Encoder;

namespace LoginServer.Cryptography.Factories
{
    public class LoginEncoderFactory : IPacketCryptoFactory
    {
        public IEncoder GetEncoder() => new NostaleLoginEncoder();

        public IDecoder GetDecoder() => new NostaleLoginDecoder();
    }
}