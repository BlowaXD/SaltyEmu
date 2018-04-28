using System;
using System.Collections.Generic;
using System.Linq;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;

namespace NosSharp.World.Session
{
    public class NetworkClient : ChannelHandlerAdapter
    {
        private readonly IChannel _channel;

        #region Members

        public bool IsAuthenticated { get; set; }

        public int SessionId { get; set; }

        public int LastKeepAliveIdentity { get; set; }

        // TODO REFACTOR THAT SHIT
        private int? _waitForPacketsAmount;
        private readonly IList<string> _waitForPacketList = new List<string>();

        public NetworkClient(IChannel channel)
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
            SessionId = 0;
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("{0}", exception.StackTrace);

            context.CloseAsync();
        }


        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (!(message is string buff))
            {
                return;
            }

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
                    // CORRUPTED KEEPALIVE
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
            // TODO look if we got the delegate for that packet header
            // TODO check if there is no packet Listener for that header
            /*
            if (methodReference == null)
            {
                // unhandled packet logg it
                return;
            }
            */

            // TODO check for waitedPacketList

            /*
            if (methodReference.PacketHeaderAttribute != null && !force && methodReference.PacketHeaderAttribute.Amount > 1 && !_waitForPacketsAmount.HasValue)
            {
                // we need to wait for more
                _waitForPacketsAmount = methodReference.PacketHeaderAttribute.Amount;
                _waitForPacketList.Add(packet != string.Empty ? packet : $"1 {packetHeader} ");
                return;
            }
            */

            // TODO check if is in CharacterLoginState to handle the right packets
            // TODO check packet permission
        }

        public void Disconnect()
        {
            _channel.DisconnectAsync();
        }


        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            SessionManager.Instance.UnregisterSession(context.Channel.Id.AsLongText());
        }

        #endregion
    }
}