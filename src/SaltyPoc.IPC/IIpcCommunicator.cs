using System;
using System.Threading.Tasks;

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
        Task<T> RequestAsync<T>(IIpcPacket packet) where T : BaseResponse;

        /// <summary>
        /// Broadcasts the given packet
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        Task SendAsync(IIpcPacket packet);
    }
}