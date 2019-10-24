using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets.Interfaces;

namespace NW.Plugins.PacketHandling.Utils
{
    public abstract class GenericSessionPacketHandlerAsync<T> : IPacketProcessor where T : class, IPacket
    {
        protected readonly ILogger Log;

        protected GenericSessionPacketHandlerAsync(ILogger log)
        {
            Log = log;
        }

        public Task Handle(IPacket packet, ISession session)
        {
            if (!(packet is T typedPacket))
            {
                return Task.CompletedTask;
            }

            return Handle(typedPacket, session);
        }

        protected abstract Task Handle(T packet, ISession session);
    }
}