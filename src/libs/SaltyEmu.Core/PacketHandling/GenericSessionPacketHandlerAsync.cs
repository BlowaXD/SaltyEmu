using System.Threading.Tasks;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets;

namespace SaltyEmu.Core.PacketHandling
{
    public abstract class GenericSessionPacketHandlerAsync<T> where T : IPacket
    {
        public Task OnPacket(IPacket packet, ISession player)
        {
            if (!(packet is T typedPacket))
            {
                return Task.CompletedTask;
            }

            return Handle(typedPacket, player);
        }

        protected abstract Task Handle(T packet, ISession player);
    }
}