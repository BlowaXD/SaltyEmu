using System;

namespace ChickenAPI.Core.ISC
{
    public interface IISCRequest
    {
        Guid Id { get; }
        IISCRequest RequestPacket { get; }
    }
}