using System;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;

namespace ChickenAPI.Core.IPC
{
    public interface IIpcRequestHandler
    {
        void Register<T>(Func<IIpcRequest, Task> handler) where T : IIpcRequest;
        void Register<T>(Func<IIpcPacket, Task> handler) where T : IIpcPacket;

        Task HandleAsync<T>(T request) where T : IIpcRequest;
        Task HandleAsync(IIpcRequest request, Type type);

        Task HandleBroadcastAsync<T>(T packet) where T : IIpcPacket;
        Task HandleBroadcastAsync(IIpcPacket packet, Type type);
    }
}