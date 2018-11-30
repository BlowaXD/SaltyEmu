using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.Communication.Configs
{
    public class MqttConfiguration
    {
        public IIpcSerializer Serializer { get; set; }
        public string EndPoint { get; set; }
        public string ClientName { get; set; }
        public string ResponseTopic { get; set; }
        public string BroadcastTopic { get; set; }
    }
}