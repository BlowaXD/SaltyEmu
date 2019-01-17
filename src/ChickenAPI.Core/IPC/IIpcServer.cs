using System;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;

namespace ChickenAPI.Core.IPC
{
    public interface IIpcServer
    {
        Task ResponseAsync<T>(T response) where T : IIpcResponse;
    }
}