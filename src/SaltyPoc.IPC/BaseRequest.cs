using System;
using System.Threading.Tasks;

namespace SaltyPoc.IPC
{
    internal class BaseRequest
    {
        public Guid Id { get; set; }

        public Type Type { get; set; }
        public string Content { get; set; }

        public TaskCompletionSource<BaseResponse> Response { get; set; }
    }
}