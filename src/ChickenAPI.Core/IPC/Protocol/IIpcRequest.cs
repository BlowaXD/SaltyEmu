using System.Threading.Tasks;

namespace ChickenAPI.Core.IPC.Protocol
{
    public interface IIpcRequest
    {
        Task ReplyAsync(IIpcResponse response);
    }
}