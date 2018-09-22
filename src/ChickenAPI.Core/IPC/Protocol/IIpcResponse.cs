using System;

namespace ChickenAPI.Core.IPC.Protocol
{
    public interface IIpcResponse : IIpcPacket
    {
        Guid RequestId { get; set; }
    }
}