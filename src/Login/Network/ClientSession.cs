using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game;
using ChickenAPI.Game.Data.AccessLayer.Account;
using ChickenAPI.Game.Data.AccessLayer.Server;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Data.TransferObjects.Server;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using DotNetty.Transport.Channels.Sockets;

namespace Login.Network
{
    public class ClientSession : ChannelHandlerAdapter
    {
        public static readonly Logger Log = Logger.GetLogger<ClientSession>();
        public static ISessionService SessionService;
        public static IAccountService AccountService;
        public static IServerApiService ServerApi;


        private static volatile IChannelGroup _group;
        private readonly ISocketChannel _channel;
        private IPEndPoint _endPoint;

        public ClientSession(ISocketChannel channel)
        {
            _channel = channel;
            _endPoint = channel.RemoteAddress as IPEndPoint;
        }

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

            _endPoint = _channel.RemoteAddress as IPEndPoint;
            Log.Info($"[{_endPoint?.Address}][SOCKET_ACCEPT] Client has been accepted");
            g.Add(context.Channel);
        }

        private string GenerateWorldListPacket(string accountName, int sessionId, IEnumerable<WorldServerDto> worlds)
        {
            string lastGroup = string.Empty;
            int worldGroupCount = 0;
            var packetBuilder = new StringBuilder();
            packetBuilder.AppendFormat("NsTeST {0} {1} ", accountName, sessionId);
            foreach (WorldServerDto world in worlds)
            {
                if (lastGroup != world.WorldGroup)
                {
                    worldGroupCount++;
                }

                lastGroup = world.WorldGroup;
                packetBuilder.AppendFormat("{0}:", world.Ip);
                packetBuilder.AppendFormat("{0}:", world.Port);
                packetBuilder.AppendFormat("{0}:", (short)world.Color);
                packetBuilder.AppendFormat("{0}.{1}.{2} ", worldGroupCount, world.ChannelId, world.WorldGroup);
            }

            packetBuilder.Append("-1:-1:-1:10000.10000.1");
            return packetBuilder.ToString();
        }

        private string GetFailPacket(AuthResponse response) => $"failc {(byte)response}";

        private void Disconnect()
        {
            Log.Info($"[{_endPoint.Address}][SOCKET_RELEASE] Client has been released");
            _channel.DisconnectAsync().Wait();
            _channel.CloseAsync().Wait();
        }

        private void SendPacket(string packet)
        {
            _channel.WriteAsync(packet).Wait();
            _channel.Flush();
        }


        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            try
            {
                if (!(message is string buff))
                {
                    return;
                }

                string[] packet = buff.Split(' ');
                if (packet.Length <= 4 || packet[0] != "NoS0575")
                {
                    Log.Info($"[{_endPoint.Address}][CONNECT_REQUEST] Wrong packet");
                    SendPacket(GetFailPacket(AuthResponse.CantConnect));
                    Disconnect();
                    return;
                }

                string accountName = packet[2];
                string passwordHash = packet[3];
                AccountDto dto = AccountService.GetByName(accountName.ToLower());
                if (dto == null)
                {
                    Log.Info($"[{_endPoint.Address}][CONNECT_REQUEST] {accountName} Not found in database");
                    SendPacket(GetFailPacket(AuthResponse.AccountOrPasswordWrong));
                    Disconnect();
                    return;
                }

                if (!string.Equals(dto.Password, passwordHash, StringComparison.CurrentCultureIgnoreCase))
                {
                    Log.Info($"[{_endPoint.Address}][CONNECT_REQUEST] {accountName} Wrong password");
                    SendPacket(GetFailPacket(AuthResponse.AccountOrPasswordWrong));
                    Disconnect();
                    return;
                }

                var response = AuthResponse.Ok;

                if (response != AuthResponse.Ok)
                {
                    Log.Info($"[{_endPoint.Address}][CONNECT_REQUEST] MAINTENANCE MODE");
                    SendPacket(GetFailPacket(AuthResponse.AccountOrPasswordWrong));
                    Disconnect();
                    return;
                }

                PlayerSessionDto session = SessionService.GetByAccountName(accountName);
                if (session != null && session.State == PlayerSessionState.Connected)
                {
                    Log.Info($"[{_endPoint.Address}][CONNECT_REQUEST] {accountName} already connected on World {session.WorldServerId}");
                    SendPacket(GetFailPacket(AuthResponse.AlreadyConnected));
                    Disconnect();
                    return;
                }


                if (session == null)
                {
                    session = new PlayerSessionDto
                    {
                        Password = passwordHash,
                        Username = accountName,
                        State = PlayerSessionState.Unauthed
                    };
                    SessionService.RegisterSession(session);
                    Log.Info($"[{_endPoint.Address}][CONNECT_ACCEPT] {accountName} waiting for world endpoint");
                }

                IEnumerable<WorldServerDto> test = ServerApi.GetServers();
                SendPacket(GenerateWorldListPacket(accountName, session.Id, test));

                Log.Info($"[{_endPoint.Address}][CONNECT_ACCEPT] Server list sent to {accountName}");
                Disconnect();
            }
            catch
            {
                _channel.WriteAndFlushAsync(GetFailPacket(AuthResponse.CantConnect)).Wait();
                _channel.DisconnectAsync().Wait();
            }
        }


        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            context.CloseAsync();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Log.Error($"[EXCEPTION] {_endPoint.Address}", exception);
            context.CloseAsync();
        }
    }
}