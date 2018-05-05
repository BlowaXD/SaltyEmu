using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ChickenAPI.Accounts;
using ChickenAPI.DAL.Interfaces;
using ChickenAPI.Dtos;
using ChickenAPI.Packets;
using ChickenAPI.Player;
using ChickenAPI.Player.Enums;
using ChickenAPI.Session;
using ChickenAPI.Utils;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;

namespace NosSharp.World.Session
{
    public class ClientSession : ChannelHandlerAdapter, ISession
    {
        private static Guid _worldServerId;
        private static IPacketFactory _packetFactory;
        private static IPacketHandler _packetHandler;
        private readonly IChannel _channel;

        #region Members

        public bool HasCurrentMapInstance => false;

        public bool IsAuthenticated => Account != null;

        public int SessionId { get; set; }
        public IPEndPoint Ip { get; private set; }

        public int LastKeepAliveIdentity { get; set; }

        // TODO REFACTOR THAT SHIT
        private int? _waitForPacketsAmount;
        private readonly IList<string> _waitForPacketList = new List<string>();

        public static void SetPacketFactory(IPacketFactory packetFactory)
        {
            _packetFactory = packetFactory;
        }

        public static void SetPacketHandler(IPacketHandler packetHandler)
        {
            _packetHandler = packetHandler;
        }

        public static void SetWorldServerId(Guid id)
        {
            _worldServerId = id;
        }

        public ClientSession(IChannel channel)
        {
            _channel = channel;
        }

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
            Ip = _channel.RemoteAddress as IPEndPoint;
            SessionId = 0;
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Logger.Log.Error(exception);
            context.CloseAsync();
        }


        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (!(message is string buff))
            {
                Logger.Log.Debug("Message is not string !");
                return;
            }

            Logger.Log.Debug($"[Session-{SessionId}] {buff}");

            if (SessionId == 0)
            {
                HandleUnauthedPackets(buff, context);
                return;
            }

            HandlePackets(buff);
        }

        private void HandleUnauthedPackets(string packets, IChannelHandlerContext context)
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
            SessionManager.Instance.RegisterSession(context.Channel.Id.AsLongText(), SessionId);
            // CLIENT ARRIVED
            if (!_waitForPacketsAmount.HasValue)
            {
                TriggerHandler("EntryPoint", string.Empty, false);
            }
        }

        private void HandlePackets(string packets)
        {
            foreach (string packet in packets.Split((char)0xFF, StringSplitOptions.RemoveEmptyEntries))
            {
                string packetstring = packet.Replace('^', ' ');
                string[] packetsplit = packetstring.Split(' ');

                // keep alive
                string nextKeepAliveRaw = packetsplit[0];
                if (!int.TryParse(nextKeepAliveRaw, out int nextKeepaliveIdentity) && nextKeepaliveIdentity != (LastKeepAliveIdentity + 1))
                {
                    Disconnect();
                    return;
                }

                if (nextKeepaliveIdentity == 0)
                {
                    if (LastKeepAliveIdentity == ushort.MaxValue)
                    {
                        LastKeepAliveIdentity = nextKeepaliveIdentity;
                    }
                }
                else
                {
                    LastKeepAliveIdentity = nextKeepaliveIdentity;
                }

                // TODO NEED TO BE REWRITED
                if (_waitForPacketsAmount.HasValue)
                {
                    _waitForPacketList.Add(packetstring);
                    string[] packetssplit = packetstring.Split(' ');
                    if (packetssplit.Length > 3 && packetsplit[1] == "DAC")
                    {
                        _waitForPacketList.Add("0 CrossServerAuthenticate");
                    }

                    if (_waitForPacketList.Count != _waitForPacketsAmount)
                    {
                        continue;
                    }

                    _waitForPacketsAmount = null;
                    string queuedPackets = string.Join(" ", _waitForPacketList.ToArray());
                    string header = queuedPackets.Split(' ', '^')[1];
                    TriggerHandler(header, queuedPackets, true);
                    _waitForPacketList.Clear();
                    return;
                }

                if (packetsplit.Length <= 1)
                {
                    continue;
                }

                if (packetsplit[1].Length >= 1 && (packetsplit[1][0] == '/' || packetsplit[1][0] == ':' || packetsplit[1][0] == ';'))
                {
                    packetsplit[1] = packetsplit[1][0].ToString();
                    packetstring = packet.Insert(packet.IndexOf(' ') + 2, " ");
                }

                if (packetsplit[1] != "0")
                {
                    TriggerHandler(packetsplit[1].Replace("#", ""), packetstring, false);
                }
            }
        }

        private void TriggerHandler(string packetHeader, string packet, bool force)
        {
            PacketHandlerMethodReference methodReference = _packetHandler.GetPacketHandlerMethodReference(packetHeader);
            Logger.Log.Debug($"PacketHeader : {packetHeader}");
            if (methodReference == null)
            {
                Logger.Log.Warn($"Handler for {packetHeader} not found !");
                return;
            }

            if (methodReference.PacketHeader != null && !force && methodReference.PacketHeader.Amount > 1 && !_waitForPacketsAmount.HasValue)
            {
                _waitForPacketsAmount = methodReference.PacketHeader.Amount;
                _waitForPacketList.Add(packet != string.Empty ? packet : $"1 {packetHeader} ");
                return;
            }

            APacket deserializedPacket = _packetFactory.Deserialize(packet, methodReference.PacketType, IsAuthenticated);

            if (deserializedPacket != null)
            {
                _packetHandler.Handle(deserializedPacket, this, methodReference.PacketType);
            }
            else
            {
                Logger.Log.Warn($"Corrupted Packet {packet}");
            }
        }

        public AccountDto Account { get; private set; }

        public bool HasSelectedCharacter => Character != null;

        public Character Character { get; private set; }

        public AuthorityType Authority => Account.Authority;

        public void SendPacket(APacket packet)
        {
            _channel.WriteAsync(_packetFactory.Serialize(packet));
            _channel.Flush();
        }

        public void SendPackets(IEnumerable<APacket> packets)
        {
            foreach (APacket packet in packets)
            {
                _channel.WriteAsync(_packetFactory.Serialize(packet));
            }

            _channel.Flush();
        }

        public void InitializeAccount(AccountDto account)
        {
            Account = account;
            PlayerSessionDto sessionDto = DependencyContainer.Instance.Get<ISessionService>().GetByName(account.Name);
            sessionDto.State = PlayerSessionState.Connected;
            sessionDto.WorldServerId = _worldServerId;
            DependencyContainer.Instance.Get<ISessionService>().UpdateSession(sessionDto);
        }

        public void InitializeCharacter(Character character)
        {
            Character = character;
        }

        public void Disconnect()
        {
            _channel.DisconnectAsync();
            DependencyContainer.Instance.Get<ISessionService>().UnregisterSession(SessionId);
        }


        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            SessionManager.Instance.UnregisterSession(context.Channel.Id.AsLongText());
            Disconnect();
            context.CloseAsync();
        }

        #endregion
    }
}