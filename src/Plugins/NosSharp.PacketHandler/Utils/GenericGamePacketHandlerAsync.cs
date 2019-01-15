using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets;

namespace NW.Plugins.PacketHandling.Utils
{
    public abstract class GenericGamePacketHandlerAsync<T> : IPacketProcessor where T : class, IPacket
    {
        protected readonly Logger Log = Logger.GetLogger<T>();

        public Task Handle(IPacket packet, ISession session)
        {
            if (!(packet is T typedPacket) || session.Player == null)
            {
                return Task.CompletedTask;
            }

            return Handle(typedPacket, session.Player);
        }

        protected abstract Task Handle(T packet, IPlayerEntity player);
    }
}