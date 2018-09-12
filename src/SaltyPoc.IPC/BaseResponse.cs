using System;

namespace SaltyPoc.IPC
{
    public class BaseResponse
    {
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
        public Type ResponseType { get; set; }
        public string Content { get; set; }
    }
}