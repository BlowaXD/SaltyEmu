using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;

namespace ChickenAPI.Core.IPC
{
    public interface IIpcClient
    {
        Task<T> RequestAsync<T>(IIpcRequest request) where T : class, IIpcResponse;
        Task BroadcastAsync<T>(T packet) where T : IIpcPacket;
    }
}