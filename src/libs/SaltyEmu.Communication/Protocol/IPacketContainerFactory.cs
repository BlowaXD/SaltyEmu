using System;
using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.Communication.Protocol
{
    public interface IPacketContainerFactory
    {
        PacketContainer Create<T>(string content);
        PacketContainer Create(Type type, string content);

        PacketContainer ToPacket<T>(T packet) where T : IAsyncRpcRequest;
        PacketContainer ToPacket(Type type, IAsyncRpcRequest packet);
    }
}