using ChickenAPI.Core.IPC;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;

namespace SaltyEmu.FamilyService
{
    public class RabbitMqFamilyServer : RabbitMqServer
    {
        public RabbitMqFamilyServer(RabbitMqConfiguration config, IIpcRequestHandler requestHandler) : base(config, requestHandler)
        {
        }
    }
}