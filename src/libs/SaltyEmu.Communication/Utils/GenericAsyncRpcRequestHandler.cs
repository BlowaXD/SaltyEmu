using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Logging;

namespace SaltyEmu.Communication.Utils
{
    public abstract class GenericAsyncRpcRequestHandler<T> : IIpcPacketHandler where T : IAsyncRpcRequest
    {
        protected readonly ILogger Log;

        protected GenericAsyncRpcRequestHandler(ILogger log)
        {
            Log = log;
        }

        public Task Handle(IAsyncRpcRequest packet)
        {
            if (packet is T request)
            {
                return Handle(request);
            }

            Log.Warn($"Packet is not of type {typeof(T)}");
            return Task.CompletedTask;
        }

        protected abstract Task Handle(T request);
    }
}