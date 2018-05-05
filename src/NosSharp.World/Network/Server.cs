using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using ChickenAPI.DAL.Interfaces;
using ChickenAPI.Dtos;
using ChickenAPI.Enums;
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
        private static bool _running { get; set; }
        public static string WorldGroup { get; set; }
        public static string Ip { get; set; }
        public static int Port { get; set; }
        public static WorldServerDto WorldServer;

        public static bool RegisterServer()
        {
            var worldServer = new WorldServerDto
            {
                WorldGroup = WorldGroup,
                Ip = Ip,
                Port = Port,
                Color = ChannelColor.White,
                Id = Guid.Empty,
                ChannelId = 0
            };
            var api = DependencyContainer.Instance.Get<IServerApiService>();
            if (api?.RegisterServer(worldServer) == true)
            {
                return true;
            }
            Server.WorldServer = worldServer;
            return false;
        }

        public static void UnregisterServer()
        {
            var api = DependencyContainer.Instance.Get<IServerApiService>();
            if (api == null)
            {
                return;
            }
            var sessionManager = DependencyContainer.Instance.Get<ISessionService>();
            api.UnregisterServer(WorldServer.Id);
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
                    .Option(ChannelOption.SoBacklog, 100)
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

                while (_running)
                {
                    string readLine = Console.ReadLine();
                }

                await bootstrapChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex);
                UnregisterServer();
            }
            finally
            {
                UnregisterServer();
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
            }
        }
    }
}