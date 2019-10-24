using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Logging;

namespace SaltyEmu.Communication.Utils
{
    public abstract class GenericIpcRequestHandler<TRequest, TResponse> : IIpcPacketHandler where TRequest : ISyncRpcRequest
                                                                                            where TResponse : ISyncRpcResponse
    {
        protected readonly ILogger Log;

        protected GenericIpcRequestHandler(ILogger log) => Log = log;

        public async Task Handle(IAsyncRpcRequest packet)
        {
            if (packet is TRequest request)
            {
                await request.ReplyAsync<TResponse, TRequest>(await Handle(request));
                return;
            }

            Log.Warn($"Packet is not of type {typeof(TRequest)}");
        }

        protected abstract Task<TResponse> Handle(TRequest request);
    }
}