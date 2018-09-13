using System;

namespace SaltyPoc.IPC.Protocol
{
    public class BaseResponse : IIpcResponse
    {
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
        public string Content { get; set; }
    }
}