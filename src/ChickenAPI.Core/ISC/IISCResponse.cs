using System;

namespace ChickenAPI.Core.ISC
{
    public interface IISCResponse
    {
        Guid RequestId { get; }
        IISCPacket ResponsePacket { get; }
    }
}