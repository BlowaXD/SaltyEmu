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

namespace LoginServer.Network
{
    public class ClientSession : ChannelHandlerAdapter
    {
        public static readonly Logger Log = Logger.GetLogger<ClientSession>();
        public static ISessionService SessionService;
        public static IAccountService AccountService;
        public static IServerApiService ServerApi;
        private readonly ISocketChannel _channel;
        private IPEndPoint _endPoint;

        public ClientSession(ISocketChannel channel)
        {
            _channel = channel;
            _endPoint = channel.RemoteAddress as IPEndPoint;
        }


        private static volatile IChannelGroup _group;

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            IChannelGroup g = _group;
            if (g == null)
            {
                lock (_channel)
                {
                    if (_group == null)
                    {
                        g = _group = new DefaultChannelGroup(context.Executor);
                    }
                }
            }

            Log.Info("Client Connected");
            _endPoint = _channel.RemoteAddress as IPEndPoint;
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

        private string GetFailPacket(AuthResponse response) => $"";

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
                    _channel.WriteAsync(GetFailPacket(AuthResponse.CantConnect)).Wait();
                    _channel.Flush();
                    _channel.DisconnectAsync().Wait();
                    context.CloseAsync().Wait();
                    return;
                }

                string accountName = packet[2];
                string passwordHash = packet[3];
                AccountDto dto = AccountService.GetByName(accountName.ToLower());
                if (dto == null)
                {
                    Log.Info($"Not found {accountName}");
                    _channel.WriteAsync("failc 5").Wait();
                    _channel.Flush();
                    _channel.DisconnectAsync().Wait();
                    context.CloseAsync().Wait();
                    return;
                }

                if (!string.Equals(dto.Password, passwordHash, StringComparison.CurrentCultureIgnoreCase))
                {
                    Log.Info($"Password does not match for {dto.Name}");
                    _channel.WriteAsync("failc 5").Wait();
                    _channel.Flush();
                    _channel.DisconnectAsync().Wait();
                    context.CloseAsync().Wait();
                    return;
                }

                var response = AuthResponse.Ok;

                if (response != AuthResponse.Ok)
                {
                    Log.Info($"Password does not match for {dto.Name}");
                    _channel.WriteAndFlushAsync(GetFailPacket(response)).Wait();
                    _channel.Flush();
                    _channel.DisconnectAsync().Wait();
                    context.CloseAsync().Wait();
                    return;
                }

                PlayerSessionDto session = SessionService.GetByAccountName(accountName);
                if (session != null && session.State == PlayerSessionState.Connected)
                {
                    Log.Info($"session already claimed");
                    _channel.WriteAndFlushAsync(GetFailPacket(AuthResponse.AlreadyConnected)).Wait();
                    _channel.Flush();
                    _channel.DisconnectAsync().Wait();
                    context.CloseAsync().Wait();
                    return;
                }

                Log.Info($"[CONNECTED] Account {accountName} with IP : {_endPoint.Address}");

                if (session == null)
                {
                    session = new PlayerSessionDto
                    {
                        Password = passwordHash,
                        Username = accountName,
                        State = PlayerSessionState.Unauthed,
                    };
                    SessionService.RegisterSession(session);
                }

                IEnumerable<WorldServerDto> test = ServerApi.GetServers();

                _channel.WriteAndFlushAsync(GenerateWorldListPacket(accountName, session.Id, test)).Wait();
                _channel.Flush();
                _channel.DisconnectAsync().Wait();
                context.CloseAsync().Wait();
            }
            catch
            {
                _channel.WriteAndFlushAsync(GetFailPacket(AuthResponse.CantConnect)).Wait();
                _channel.DisconnectAsync().Wait();
            }
        }


        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            Log.Info("[DISCONNECT]");
            context.CloseAsync();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Log.Error($"[EXCEPTION] {_endPoint.Address}", exception);
            context.CloseAsync();
        }
    }
}