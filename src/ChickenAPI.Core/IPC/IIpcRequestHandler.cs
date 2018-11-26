using System;
using ChickenAPI.Core.IPC.Protocol;

namespace ChickenAPI.Core.IPC
{
    public interface IIpcRequestHandler
    {
        void Register<T>(Action<T> handler) where T : IIpcRequest;
        void Handle(IIpcRequest request, Type type);
        void Handle<T>(T request) where T : IIpcRequest;
    }
}