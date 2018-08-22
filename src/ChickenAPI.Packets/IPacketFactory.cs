using System;

namespace ChickenAPI.Packets
{
    public interface IPacketFactory
    {
        string Serialize<TPacket>(TPacket packet) where TPacket : IPacket;

        string Serialize(IPacket packet, Type type);

        IPacket Deserialize(string packetContent, Type packetType, bool includesKeepAliveIdentity);
    }
}