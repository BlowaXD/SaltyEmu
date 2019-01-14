using System.Threading.Tasks;

namespace ChickenAPI.Core.IPC.Protocol
{
    public interface IIpcRequest : IIpcPacket
    {
        string ResponseTopic { get; }
        string RequestTopic { get; }
        Task ReplyAsync<T>(T response) where T : IIpcResponse;
    }
}