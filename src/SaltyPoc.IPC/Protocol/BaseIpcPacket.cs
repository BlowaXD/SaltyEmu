using System;

namespace SaltyPoc.IPC.Protocol
{
    public class BaseIpcPacket : IIpcPacket
    {
        public Guid Id { get; set; }
    }
}