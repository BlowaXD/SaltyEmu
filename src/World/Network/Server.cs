using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Server;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Data.AccessLayer.Server;
using ChickenAPI.Game.ECS;
using ChickenAPI.Game.ECS.Entities;
using DotNetty.Common.Concurrency;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace World.Network
{
    public class Server
    {
        // Server Tick config 
        private static readonly Logger Log = Logger.GetLogger<Server>();
        public static WorldServerDto WorldServer;

        private static IEntityManagerContainer _container;
        private static bool _running { get; set; }
        public static string WorldGroup { get; set; }
        public static string Ip { get; set; }
        public static int Port { get; set; }
        public static int TickRate { get; set; }

        private static long DelayBetweenTicks => 1000 / TickRate;

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
            var api = ChickenContainer.Instance.Resolve<IServerApiService>();
            if (api?.RegisterServer(worldServer) == true)
            {
                return true;
            }

            WorldServer = worldServer;
            ClientSession.SetWorldServerId(WorldServer.Id);
            _running = true;
            Log.Info($"Registering server {WorldServer.Id}");
            return false;
        }

        public static void UnregisterServer()
        {
            if (_running == false)
            {
                return;
            }

            var api = ChickenContainer.Instance.Resolve<IServerApiService>();
            if (api == null)
            {
                return;
            }

            Log.Info($"Unregister server {WorldServer.Id}");

            var sessionManager = ChickenContainer.Instance.Resolve<ISessionService>();
            api.UnregisterServer(WorldServer.Id);
            sessionManager.UnregisterSessions(WorldServer.Id);
            _running = false;
        }

        private static void ServerLoop()
        {
            _container = ChickenContainer.Instance.Resolve<IEntityManagerContainer>();
            while (_running)
            {
                try
                {
                    DateTime next = DateTime.UtcNow.AddMilliseconds(DelayBetweenTicks);
                    Update();
                    DateTime after = DateTime.UtcNow;

                    if (next > after)
                    {
                        Thread.Sleep((next - after).Milliseconds);
                    }
                }
                catch (Exception e)
                {
                    Log.Error("[SERVER_LOOP]", e);
                }
            }
        }

        private static void Update()
        {
            foreach (IEntityManager manager in _container.Managers)
            {
                try
                {
                    DateTime tmp = DateTime.UtcNow;
                    manager.Update(tmp);
                }
                catch (Exception e)
                {
                    Log.Error("[TICK]", e);
                    _container.Unregister(manager);
                }
            }
        }

        private static void SetupServerLoop(IEventExecutor eventExecutor)
        {
            if (!_running)
            {
                return;
            }

            eventExecutor.Execute(() =>
            {
                eventExecutor.Schedule(() => { SetupServerLoop(eventExecutor); }, TimeSpan.FromMilliseconds(DelayBetweenTicks));
                Update();
            });
        }

        public static async Task RunServerAsync(int port)
        {
            var bossGroup = new MultithreadEventLoopGroup();
            var workerGroup = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap
                    .Option(ChannelOption.SoBacklog, 100)
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .ChildHandler(new ClientSessionInitializer());

                IChannel bootstrapChannel = await bootstrap.BindAsync(port).ConfigureAwait(false);


                Log.Info("[LISTENING] Server is listening");
                Log.Info($"-> PORT   : {port}");
                Log.Info($"-> TICK   : {TickRate}");
                Log.Info($"-> WORLD  : {WorldGroup}");
                ServerLoop();

                await bootstrapChannel.CloseAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error("RunServerAsync", ex);
            }
            finally
            {
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
            }
        }
    }
}