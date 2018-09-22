using System;

namespace SaltyEmu.IpcPlugin.Protocol
{
    public interface IPacketContainerFactory
    {
        PacketContainer Create<T>(string content);
        PacketContainer Create(Type type, string content);
    }
}