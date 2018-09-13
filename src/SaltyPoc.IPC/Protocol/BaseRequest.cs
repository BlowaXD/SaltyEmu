using System;
using System.Threading.Tasks;

namespace SaltyPoc.IPC.Protocol
{
    internal class BaseRequest : IIpcRequest
    {
        public Guid Id { get; set; }

        public Task RespondAsync<T>(T obj) where T : IIpcResponse
        {
            return Task.CompletedTask;
        }

        public Type Type { get; set; }
        public string Content { get; set; }

        public TaskCompletionSource<BaseResponse> Response { get; set; }
    }
}