using System;
using System.Threading.Tasks;
using ChickenAPI.Game.Network;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.PacketHandling
{
    public interface IPacketPipelineAsync
    {
        Task RegisterAsync(IPacketProcessor processor, Type packetType);

        Task UnregisterAsync(IPacketProcessor processor, Type packetType);

        Task Handle(IPacket packet, ISession session);

        Type PacketTypeByHeader(string header);
    }
}