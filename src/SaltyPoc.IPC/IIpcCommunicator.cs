using System.Threading.Tasks;
using SaltyPoc.IPC.Protocol;

namespace SaltyPoc.IPC
{
    public interface IIpcCommunicator
    {
        /// <summary>
        /// Requests and wait for a specific response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packet"></param>
        /// <returns></returns>
        Task<T> RequestAsync<T>(IIpcRequest packet) where T : IIpcResponse;

        /// <summary>
        /// Broadcasts the given packet
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        Task SendAsync(IIpcPacket packet);

        Task<T> RespondAsync<T>(T packet) where T : IIpcResponse;
    }
}