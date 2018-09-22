using System.Threading.Tasks;

namespace ChickenAPI.Core.IPC.Protocol
{
    public interface IIpcRequest : IIpcPacket
    {
        Task ReplyAsync(IIpcResponse response);
    }
}