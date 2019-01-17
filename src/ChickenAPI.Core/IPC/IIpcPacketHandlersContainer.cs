using System;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;

namespace ChickenAPI.Core.IPC
{
    public interface IIpcPacketHandlersContainer
    {
        event EventHandler<Type> Registered;
        event EventHandler<Type> Unregistered;
        Task RegisterAsync(IIpcPacketHandler handler, Type type);

        Task UnregisterAsync(Type type);
        Task HandleAsync(IIpcPacket request, Type type);
    }
}