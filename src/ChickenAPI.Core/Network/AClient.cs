using System;
using System.Collections.Generic;
using ChickenAPI.Core.Utils;

namespace ChickenAPI.Core.Network
{
    public abstract class AClient<TClient, TPacket> : IClient<TPacket> where TClient : class
    {
        public int Id { get; set; }

        public static event TypedSenderEventHandler<TClient, EventArgs> PacketReceived;
        public static event TypedSenderEventHandler<TClient, EventArgs> PacketSent;
        public static event TypedSenderEventHandler<TClient, EventArgs> Disconnected;
        public static event TypedSenderEventHandler<TClient, EventArgs> Connected;

        public void SendPackets(IEnumerable<TPacket> packets)
        {
            foreach (TPacket packet in packets)
            {
                SendPacket(packet);
            }
        }

        public void Disconnect()
        {
            Disconnected?.Invoke(this as TClient, new EventArgs());
        }

        public void SendPacket(TPacket packet)
        {
            PacketSent?.Invoke(this as TClient, packet as EventArgs);
        }
    }
}