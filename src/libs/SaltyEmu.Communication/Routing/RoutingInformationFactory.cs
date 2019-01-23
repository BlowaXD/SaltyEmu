using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.Logging;

namespace SaltyEmu.Communication
{
    public class RoutingInformationFactory : IRoutingInformationFactory
    {
        private readonly Logger Log = Logger.GetLogger<RoutingInformationFactory>();
        public Task<IRoutingInformation> Create(string topic, string responseTopic)
        {
            Log.Info($"[PACKET] Routing to : {topic}");
            return Task.FromResult((IRoutingInformation)new RoutingInformation
            {
                IncomingTopic = topic,
                OutgoingTopic = responseTopic
            });
        }
    }
}