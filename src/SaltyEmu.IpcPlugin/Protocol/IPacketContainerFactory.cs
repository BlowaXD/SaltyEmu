using System;
using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.IpcPlugin.Protocol
{
    public interface IPacketContainerFactory
    {
        PacketContainer Create<T>(string content);
        PacketContainer Create(Type type, string content);

        PacketContainer ToPacket<T>(IIpcPacket packet);
        PacketContainer ToPacket(Type type, IIpcPacket packet);
    }
}