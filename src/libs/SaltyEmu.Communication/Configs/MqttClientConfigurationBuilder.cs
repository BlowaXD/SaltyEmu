using System.Collections.Generic;
using ChickenAPI.Core.IPC;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.Communication.Configs
{
    public class MqttClientConfigurationBuilder
    {
        private readonly HashSet<string> _subscribedQueues = new HashSet<string>();
        private string _endpoint;
        private IIpcSerializer _serializer;
        private string _clientName;
        private string _responseTopic;
        private string _requestTopic;

        public MqttClientConfigurationBuilder WithName(string name)
        {
            _clientName = name;
            return this;
        }

        public MqttClientConfigurationBuilder ConnectTo(string endpoint)
        {
            _endpoint = endpoint;
            return this;
        }

        public MqttClientConfigurationBuilder WithQueueName(string requestQueue)
        {
            _subscribedQueues.Add(requestQueue);
            return this;
        }

        public MqttClientConfigurationBuilder WithSerializer(IIpcSerializer serializer)
        {
            _serializer = serializer;
            return this;
        }

        public MqttClientConfigurationBuilder WithResponseTopic(string responseTopic)
        {
            _responseTopic = responseTopic;
            return this;
        }

        public MqttClientConfigurationBuilder WithRequestTopic(string requestTopic)
        {
            _requestTopic = requestTopic;
            return this;
        }

        public MqttIpcClientConfiguration Build()
        {
            return new MqttIpcClientConfiguration
            {
                ClientName = _clientName,
                EndPoint = _endpoint,
                Serializer = _serializer,
                ResponseTopic = _responseTopic,
                RequestTopic = _requestTopic
            };
        }

        public static implicit operator MqttIpcClientConfiguration(MqttClientConfigurationBuilder builder)
        {
            return builder.Build();
        }
    }
}