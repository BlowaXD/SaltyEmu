using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using WingsEmu.World.Cryptography;

namespace WingsEmu.World.Network
{
    public class ClientSessionInitializer : ChannelInitializer<ISocketChannel>
    {
        private readonly IPacketCryptoFactory _factory = new WorldCryptoFactory();

        protected override void InitChannel(ISocketChannel channel)
        {
            // get the channel pipeline
            IChannelPipeline pipeline = channel.Pipeline;
            pipeline.AddLast("decoder", (MessageToMessageDecoder<IByteBuffer>)_factory.GetDecoder());
            pipeline.AddLast("encoder", (MessageToMessageEncoder<string>)_factory.GetEncoder());
            pipeline.AddLast("session", new ClientSession(channel));
        }
    }
}