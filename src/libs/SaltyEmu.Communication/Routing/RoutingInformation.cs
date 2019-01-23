using ChickenAPI.Core.IPC;

namespace SaltyEmu.Communication
{
    public class RoutingInformation : IRoutingInformation
    {
        public string IncomingTopic { get; set; }
        public string OutgoingTopic { get; set; }
    }
}