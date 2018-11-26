using System.Collections.Generic;
using ChickenAPI.Core.IPC;

namespace SaltyEmu.Communication.Configs
{
    public class MqttIpcServerConfiguration : MqttConfiguration
    {
        public ICollection<string> SubscribingQueues { get; set; }
        public IIpcRequestHandler Handler { get; set; }
    }
}