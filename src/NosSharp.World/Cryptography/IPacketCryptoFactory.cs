using NosSharp.World.Cryptography.Decoder;
using NosSharp.World.Cryptography.Encoder;

namespace NosSharp.World.Cryptography
{
    public interface IPacketCryptoFactory
    {
        IEncoder GetEncoder();
        IDecoder GetDecoder();
    }
}