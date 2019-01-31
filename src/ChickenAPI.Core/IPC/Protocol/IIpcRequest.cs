using System.Threading.Tasks;

namespace ChickenAPI.Core.IPC.Protocol
{
    public interface IIpcRequest : IIpcPacket
    {
        Task ReplyAsync<T, TRequest>(T response) where T : IIpcResponse;
    }
}