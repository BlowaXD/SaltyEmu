using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.i18n;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Account;
using ChickenAPI.Data.Server;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Managers;
using ChickenAPI.Packets;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using SaltyEmu.Commands.Interfaces;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets.Attributes;
using ChickenAPI.Packets.Interfaces;
using SaltyEmu.Core.Logging;

namespace World.Network
{
    public class ClientSession : ChannelHandlerAdapter, ISession
    {
        private static readonly IPlayerManager PlayerManager = new Lazy<IPlayerManager>(ChickenContainer.Instance.Resolve<IPlayerManager>).Value;
        private static readonly ISessionService SessionService = new Lazy<ISessionService>(ChickenContainer.Instance.Resolve<ISessionService>).Value;
        private static readonly ICommandContainer Commands = new Lazy<ICommandContainer>(ChickenContainer.Instance.Resolve<ICommandContainer>).Value;

        private static readonly IPacketPipelineAsync PacketPipeline = new Lazy<IPacketPipelineAsync>(ChickenContainer.Instance.Resolve<IPacketPipelineAsync>).Value;
        private static readonly IDeserializer packetDeserializer = new Lazy<IDeserializer>(ChickenContainer.Instance.Resolve<IDeserializer>).Value;
        private static readonly ISerializer packetSerializer = new Lazy<ISerializer>(ChickenContainer.Instance.Resolve<ISerializer>).Value;

        private static readonly Logger Log = Logger.GetLogger<ClientSession>();
        private static Guid _worldServerId;
        private readonly IChannel _channel;

        #region Members

        public long CharacterId { get; private set; }
        public bool IsAuthenticated => Account != null;

        public int SessionId { get; set; }
        public IPAddress Ip { get; private set; }
        public AccountDto Account { get; private set; }
        public IPlayerEntity Player { get; private set; }
        public LanguageKey Language => Account?.Language ?? LanguageKey.EN;

        public int LastKeepAliveIdentity { get; set; }

        // TODO REFACTOR THAT SHIT
        private int? _waitForPacketsAmount;
        private readonly IList<string> _waitForPacketList = new List<string>();

        public static void SetWorldServerId(Guid id)
        {
            _worldServerId = id;
        }

        public ClientSession(IChannel channel) => _channel = channel;

        #endregion

        #region Methods

        private static volatile IChannelGroup _group;

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            IChannelGroup g = _group;
            if (g == null)
            {
                lock(_channel)
                {
                    if (_group == null)
                    {
                        g = _group = new DefaultChannelGroup(context.Executor);
                    }
                }
            }

            g.Add(context.Channel);
            Ip = (_channel.RemoteAddress as IPEndPoint).Address.MapToIPv4();

#if DEBUG
            Log.Debug($"[{Ip}][SOCKET_ACCEPT]");
#endif
            SessionId = 0;
        }


        public void InitializeCharacterId(long id)
        {
            CharacterId = id;
        }

        public void InitializeEntity(IPlayerEntity ett)
        {
            Player = ett;
        }

        public void InitializeAccount(AccountDto account)
        {
            Account = account;
            RegisterSession();
        }

        private void RegisterSession()
        {
            PlayerSessionDto tmp = SessionService.GetByAccountName(Account.Name);
            tmp.WorldServerId = _worldServerId;
            SessionService.UpdateSession(tmp);
        }

        private static readonly string[] BlacklistedDebug = { "mv", "cond", "in ", "st" };

        public void SendPacket<T>(T packet) where T : IPacket
        {
            if (packet == null)
            {
                return;
            }

            string tmp = packetSerializer.Serialize(packet);

#if DEBUG
            if (BlacklistedDebug.All(s => !tmp.StartsWith(s)))
            {
                Log.Debug($"[{Ip}][SOCKET_WRITE] {tmp}");
            }
#endif

            _channel.WriteAsync(tmp);
            _channel.Flush();
        }

        public void SendPackets<T>(IEnumerable<T> packets) where T : IPacket
        {
            try
            {
                if (packets == null)
                {
                    return;
                }

                foreach (T packet in packets)
                {
                    if (packet == null)
                    {
                        continue;
                    }

                    string tmp = packetSerializer.Serialize(packet);
#if DEBUG
                    if (BlacklistedDebug.All(s => !tmp.StartsWith(s)))
                    {
                        Log.Debug($"[{Ip}][SOCKET_WRITE] {tmp}");
                    }
#endif

                    _channel.WriteAsync(tmp);
                }

                _channel.Flush();
            }
            catch (Exception e)
            {
                Log.Error("[SendPackets<ChildType>]", e);
                Disconnect();
            }
        }

        public void SendPackets(IEnumerable<IPacket> packets)
        {
            try
            {
                foreach (IPacket packet in packets)
                {
                    if (packet == null)
                    {
                        continue;
                    }

                    string tmp = packetSerializer.Serialize(packet);
#if DEBUG
                    if (BlacklistedDebug.All(s => !tmp.StartsWith(s)))
                    {
                        Log.Debug($"[{Ip}][SOCKET_WRITE] {tmp}");
                    }
#endif

                    _channel.WriteAsync(tmp);
                }

                _channel.Flush();
            }
            catch (Exception e)
            {
                Log.Error("[SendPackets(IPacket)]", e);
                Disconnect();
            }
        }

        public Task SendPacketAsync<T>(T packet) where T : IPacket
        {
            if (packet == null)
            {
                return Task.CompletedTask;
            }

            string tmp = packetSerializer.Serialize(packet);

#if DEBUG
            if (BlacklistedDebug.All(s => !tmp.StartsWith(s)))
            {
                Log.Debug($"[{Ip}][SOCKET_WRITE] {tmp}");
            }
#endif

            Task task = _channel.WriteAsync(tmp);
            _channel.Flush();
            return Task.CompletedTask;
        }

        public Task SendPacketsAsync<T>(IEnumerable<T> packets) where T : IPacket
        {
            try
            {
                if (packets == null)
                {
                    return Task.CompletedTask;
                }

                foreach (T packet in packets)
                {
                    if (packet == null)
                    {
                        continue;
                    }

                    string tmp = packetSerializer.Serialize(packet);
#if DEBUG
                    if (BlacklistedDebug.All(s => !tmp.StartsWith(s)))
                    {
                        Log.Debug($"[{Ip}][SOCKET_WRITE] {tmp}");
                    }
#endif

                    _channel.WriteAsync(tmp);
                }

                _channel.Flush();
            }
            catch (Exception e)
            {
                Log.Error("[SendPackets<ChildType>]", e);
                Disconnect();
            }

            return Task.CompletedTask;
        }

        public Task SendPacketsAsync(IEnumerable<IPacket> packets)
        {
            try
            {
                foreach (IPacket packet in packets)
                {
                    if (packet == null)
                    {
                        continue;
                    }

                    string tmp = packetSerializer.Serialize(packet);
#if DEBUG
                    if (BlacklistedDebug.All(s => !tmp.StartsWith(s)))
                    {
                        Log.Debug($"[{Ip}][SOCKET_WRITE] {tmp}");
                    }
#endif

                    _channel.WriteAsync(tmp);
                }

                _channel.Flush();
            }
            catch (Exception e)
            {
                Log.Error("[SendPackets(IPacket)]", e);
                Disconnect();
            }

            return Task.CompletedTask;
        }


        public void Disconnect()
        {
#if DEBUG
            Log.Debug($"[{Ip}][SOCKET_RELEASE]");
#endif

            Log.Info($"[DISCONNECT] {Ip}");

            PlayerManager.UnregisterPlayer(Player);
            Player?.CurrentMap.UnregisterEntity(Player);
            Player?.Save();
            Player?.Dispose();
            Player = null;
            SessionService.UnregisterSession(SessionId);
            _channel.DisconnectAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }


        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            Disconnect();
            SocketSessionManager.Instance.UnregisterSession(context.Channel.Id.AsLongText());
            context.CloseAsync();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Disconnect();
#if DEBUG
            Log.Error($"[{Ip}][SOCKET_EXCEPTION]", exception);
#endif
            context.CloseAsync();
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            try
            {
                if (!(message is string buff))
                {
                    return;
                }

                if (SessionId == 0)
                {
                    BufferToUnauthedPackets(buff, context);
                    return;
                }
            }
            catch (Exception e)
            {
                Log.Error("[ChannelRead]", e);
            }
        }

        private void BufferToUnauthedPackets(string packets, IChannelHandlerContext context)
        {
            string[] sessionParts = packets.Split(' ');
            if (sessionParts.Length == 0)
            {
                return;
            }

            if (!int.TryParse(sessionParts[0], out int lastKeepAlive))
            {
                Disconnect();
            }

            LastKeepAliveIdentity = lastKeepAlive;
            if (sessionParts.Length < 2)
            {
                return;
            }

            if (!int.TryParse(sessionParts[1].Split('\\').FirstOrDefault(), out int sessid))
            {
                return;
            }

            SessionId = sessid;
            SocketSessionManager.Instance.RegisterSession(context.Channel.Id.AsLongText(), this);
            // CLIENT ARRIVED
        }


        private void KeepAliveCheck(int nextKeepAlive)
        {
            if (nextKeepAlive == 0)
            {
                if (LastKeepAliveIdentity == ushort.MaxValue)
                {
                    LastKeepAliveIdentity = nextKeepAlive;
                }
            }
            else
            {
                LastKeepAliveIdentity = nextKeepAlive;
            }
        }

        private const string CommandPrefix = "$";
    }

    #endregion
}