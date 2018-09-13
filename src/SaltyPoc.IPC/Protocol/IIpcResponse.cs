using System;

namespace SaltyPoc.IPC.Protocol
{
    public interface IIpcResponse : IIpcPacket
    {
        Guid RequestId { get; set; }
    }
}