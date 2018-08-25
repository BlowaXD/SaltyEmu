using System;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Login.Network;
using LoginServer.Cryptography.Factories;

namespace LoginServer.Network
{
    public class NetworkManager
    {
        public static readonly Logger Log = Logger.GetLogger<NetworkManager>();

        public static async Task RunServerAsync(int port, IPacketCryptoFactory factory)
        {
            var bossGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast((MessageToMessageEncoder<string>)factory.GetEncoder());
                        pipeline.AddLast((MessageToMessageDecoder<IByteBuffer>)factory.GetDecoder());
                        pipeline.AddLast(new ClientSession(channel));
                    }));

                IChannel bootstrapChannel = await bootstrap.BindAsync(port).ConfigureAwait(false);

                Log.Info($"[LISTENING] Server is listening");
                Log.Info($"-> PORT : {port}");

                while (Console.ReadLine() != "quit")
                {
                    Thread.Sleep(2000);
                }

                await bootstrapChannel.CloseAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error("[SERVER]", ex);
            }
            finally
            {
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
            }
        }
    }
}