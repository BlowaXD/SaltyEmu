using System;

namespace ChickenAPI.Core.IPC.Protocol
{
    public interface IIpcPacket
    {
        Guid Id { get; set; }
    }
}