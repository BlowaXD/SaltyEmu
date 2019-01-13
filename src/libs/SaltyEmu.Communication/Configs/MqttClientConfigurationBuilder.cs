using System.Collections.Generic;
using ChickenAPI.Core.IPC;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.Communication.Configs
{
    public class MqttClientConfigurationBuilder
    {
        private string _endpoint;
        private IIpcSerializer _serializer;
        private string _clientName;
        private string _responseTopic;
        private string _requestTopic;
        private string _broadcastTopic;

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

        public MqttClientConfigurationBuilder WithBroadcastTopic(string broadcastTopic)
        {
            _broadcastTopic = broadcastTopic;
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
                RequestTopic = _requestTopic,
                BroadcastTopic = _broadcastTopic
            };
        }

        public static implicit operator MqttIpcClientConfiguration(MqttClientConfigurationBuilder builder)
        {
            return builder.Build();
        }
    }
}