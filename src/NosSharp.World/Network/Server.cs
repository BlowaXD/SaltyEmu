using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using ChickenAPI.DAL.Interfaces;
using ChickenAPI.Dtos;
using ChickenAPI.Utils;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NosSharp.World.Cryptography;
using NosSharp.World.Session;

namespace NosSharp.World.Network
{
    public class Server
    {
        public static WorldServerDto WorldServer;

        public static void UnregisterServer()
        {
            var api = DependencyContainer.Instance.Get<IServerApiService>();
            var sessionManager = DependencyContainer.Instance.Get<ISessionService>();
            api.UnregisterServer(WorldServer);
            sessionManager.UnregisterSessions(WorldServer.Id);
        }

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
                        pipeline.AddLast("decoder", (MessageToMessageDecoder<IByteBuffer>)factory.GetDecoder());
                        pipeline.AddLast("encoder", (MessageToMessageEncoder<string>)factory.GetEncoder());
                        pipeline.AddLast("session", new ClientSession(channel));
                    }));

                    IChannel bootstrapChannel = await bootstrap.BindAsync(port);

                Console.ReadLine();
                await bootstrapChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                UnregisterServer();
                //Log.Error(ex.Message);
            }
            finally
            {
                UnregisterServer();
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
            }
        }
    }
}