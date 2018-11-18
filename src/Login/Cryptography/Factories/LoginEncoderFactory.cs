using Login.Cryptography.Decoder;
using Login.Cryptography.Encoder;

namespace Login.Cryptography.Factories
{
    public class LoginEncoderFactory : IPacketCryptoFactory
    {
        public IEncoder GetEncoder() => new NostaleLoginEncoder();

        public IDecoder GetDecoder() => new NostaleLoginDecoder();
    }
}