using System.Threading.Tasks;
using ChickenAPI.Game.Network;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.PacketHandling
{
    public interface IPacketProcessor
    {
        Task Handle(IPacket packet, ISession session);
    }
}