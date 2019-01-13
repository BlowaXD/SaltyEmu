using ChickenAPI.Core.Events;

namespace SaltyEmu.Communication.Protocol
{
    public class BroadcastEventPacket<T> : BaseBroadcastedPacket
    where T : ChickenEvent
    {
        public T Notification { get; set; }
    }
}