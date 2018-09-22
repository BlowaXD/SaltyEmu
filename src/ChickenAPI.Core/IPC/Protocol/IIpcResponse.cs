using System;

namespace ChickenAPI.Core.IPC.Protocol
{
    public interface IIpcResponse
    {
        Guid RequestId { get; set; }
    }
}