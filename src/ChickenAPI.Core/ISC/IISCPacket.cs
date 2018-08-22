using System;

namespace ChickenAPI.Core.ISC
{
    public interface IISCPacket
    {
        Type PacketType { get; }
        string Content { get; }
    }
}