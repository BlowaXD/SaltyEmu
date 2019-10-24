using System.Threading.Tasks;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Interfaces;

namespace ChickenAPI.Game._Network
{
    public interface IPacketProcessor
    {
        Task Handle(IPacket packet, ISession session);
    }
}