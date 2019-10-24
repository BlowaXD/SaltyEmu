using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Interfaces;

namespace SaltyEmu.Core.PacketHandling
{
    public abstract class GenericGamePacketHandlerAsync<T>
    {
        public Task OnPacket(IPacket packet, IPlayerEntity player)
        {
            if (!(packet is T typedPacket))
            {
                return Task.CompletedTask;
            }

            return Handle(typedPacket, player);
        }

        protected abstract Task Handle(T packet, IPlayerEntity player);
    }
}