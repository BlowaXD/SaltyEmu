using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Logging;

namespace SaltyEmu.Communication.Utils
{
    public abstract class GenericIpcPacketHandler<T> : IIpcPacketHandler where T : IIpcPacket
    {
        protected readonly Logger Log = Logger.GetLogger<GenericIpcPacketHandler<T>>();

        public Task Handle(IIpcPacket packet)
        {
            if (packet is T request)
            {
                return Handle(request);
            }

            Log.Warn($"Packet is not of type {typeof(T)}");
            return Task.CompletedTask;
        }

        protected abstract Task Handle(T request);
    }
}