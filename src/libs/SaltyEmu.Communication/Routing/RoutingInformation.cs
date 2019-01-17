using ChickenAPI.Core.IPC;

namespace SaltyEmu.Communication
{
    public class RoutingInformation : IRoutingInformation
    {
        public string Topic { get; set; }
        public string ResponseTopic { get; set; }
    }
}