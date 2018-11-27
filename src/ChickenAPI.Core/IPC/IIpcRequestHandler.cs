using System;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;

namespace ChickenAPI.Core.IPC
{
    public interface IIpcRequestHandler
    {
        void Register<T>(Func<IIpcRequest, Task> handler) where T : IIpcRequest;

        Task HandleAsync<T>(T request) where T : IIpcRequest;
        Task HandleAsync(IIpcRequest request, Type type);
    }
}