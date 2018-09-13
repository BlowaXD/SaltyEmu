using System.Threading.Tasks;

namespace SaltyPoc.IPC.Protocol
{
    public interface IIpcRequest : IIpcPacket
    {
        Task RespondAsync<T>(T obj) where T : IIpcResponse;
    }
}