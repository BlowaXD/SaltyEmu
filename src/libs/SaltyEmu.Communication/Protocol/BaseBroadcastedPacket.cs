using System;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Data.Server;

namespace SaltyEmu.Communication.Protocol
{
    public abstract class BaseBroadcastedPacket : IAsyncRpcRequest
    {
        public Guid Id { get; set; }

        public WorldServerDto Sender { get; set; }

        public Type PacketType { get; set; }

        public string Packet { get; set; }
    }
}