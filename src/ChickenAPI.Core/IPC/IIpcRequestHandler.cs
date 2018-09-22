using System;
using ChickenAPI.Core.IPC.Protocol;

namespace ChickenAPI.Core.IPC
{
    public interface IIpcRequestHandler
    {
        Type Type { get; }
        void Handle(IIpcRequest request);
    }
}