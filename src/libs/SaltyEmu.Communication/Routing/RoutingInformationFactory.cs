using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.Logging;

namespace SaltyEmu.Communication
{
    public class RoutingInformationFactory : IRoutingInformationFactory
    {
        private readonly ILogger _log;

        public RoutingInformationFactory(ILogger log)
        {
            _log = log;
        }

        public Task<IRoutingInformation> Create(string topic, string responseTopic)
        {
            _log.Info($"[PACKET] Routing to : {topic}");
            return Task.FromResult((IRoutingInformation)new RoutingInformation
            {
                IncomingTopic = topic,
                OutgoingTopic = responseTopic
            });
        }
    }
}