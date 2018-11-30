using System;
using System.Collections.Generic;
using System.Net;
using ChickenAPI.Core.IPC;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.Communication.Configs
{
    public class MqttServerConfigurationBuilder
    {
        private readonly HashSet<string> _subscribedQueues = new HashSet<string>();
        private string _endpoint;
        private IIpcSerializer _serializer;
        private string _clientName;
        private string _responseTopic;
        private IIpcRequestHandler _handler;
        private string _broadcastTopic;

        public MqttServerConfigurationBuilder WithName(string name)
        {
            _clientName = name;
            return this;
        }

        public MqttServerConfigurationBuilder ConnectTo(string endpoint)
        {
            _endpoint = endpoint;
            return this;
        }

        public MqttServerConfigurationBuilder AddTopic(string requestQueue)
        {
            _subscribedQueues.Add(requestQueue);
            return this;
        }

        public MqttServerConfigurationBuilder WithSerializer(IIpcSerializer serializer)
        {
            _serializer = serializer;
            return this;
        }

        public MqttServerConfigurationBuilder WithResponseTopic(string responseTopic)
        {
            _responseTopic = responseTopic;
            return this;
        }

        public MqttServerConfigurationBuilder WithRequestHandler(IIpcRequestHandler handler)
        {
            _handler = handler;
            return this;
        }

        public MqttServerConfigurationBuilder WithBroadcastTopic(string broadcastTopic)
        {
            _broadcastTopic = broadcastTopic;
            return this;
        }

        public MqttIpcServerConfiguration Build()
        {
            return new MqttIpcServerConfiguration
            {
                SubscribingQueues = _subscribedQueues,
                ClientName = _clientName,
                EndPoint= _endpoint,
                Serializer = _serializer,
                ResponseTopic = _responseTopic,
                BroadcastTopic = _broadcastTopic,
                Handler = _handler
            };
        }

        public static implicit operator MqttIpcServerConfiguration(MqttServerConfigurationBuilder builder)
        {
            return builder.Build();
        }
    }
}