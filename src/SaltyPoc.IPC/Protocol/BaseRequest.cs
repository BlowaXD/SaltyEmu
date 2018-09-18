using System;
using System.Threading.Tasks;

namespace SaltyPoc.IPC.Protocol
{
    public class BaseRequest : IIpcRequest
    {
        public Guid Id { get; set; }

        public async Task RespondAsync<T>(T obj) where T : IIpcResponse
        {
            obj.RequestId = Id;
            await Communicator.RespondAsync(obj);
        }

        public Type Type { get; set; }
        public string Content { get; set; }
        public Type ResponseType { get;set; }

        public TaskCompletionSource<BaseResponse> Response { get; set; }
        internal IIpcCommunicator Communicator { get; set; }
    }
}