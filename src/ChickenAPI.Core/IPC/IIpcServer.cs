using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;

namespace ChickenAPI.Core.IPC
{
    public interface IIpcServer
    {
        Task RegisterRequestsAsync<T>(T packet) where T : IIpcRequest;
        Task RegisterPacketsAsync<T>(T packet) where T : IIpcPacket;
        Task ResponseAsync<T>(T response) where T : IIpcResponse;
    }
}