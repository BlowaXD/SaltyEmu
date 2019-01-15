using System;
using System.Threading.Tasks;
using ChickenAPI.Packets;

namespace ChickenAPI.Game._Network
{
    public interface IPacketPipelineAsync
    {
        Task RegisterAsync(IPacketProcessor processor, Type packetType);

        Task UnregisterAsync(IPacketProcessor processor, Type packetType);

        Task Handle(IPacket packet, ISession session);

        Type PacketTypeByHeader(string header);
    }
}