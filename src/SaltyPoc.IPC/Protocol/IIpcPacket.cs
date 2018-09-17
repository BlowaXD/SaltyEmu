using System;

namespace SaltyPoc.IPC.Protocol
{
    public interface IIpcPacket
    {
        Guid Id { get; set; }
    }
}