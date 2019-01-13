using System;

namespace ChickenAPI.Packets
{
    public interface IPacketFactory
    {
        string Serialize(IPacket packet);

        string Serialize<TPacket>(TPacket packet) where TPacket : IPacket;

        IPacket Deserialize(string packetContent, Type packetType, bool includesKeepAliveIdentity);
    }
}