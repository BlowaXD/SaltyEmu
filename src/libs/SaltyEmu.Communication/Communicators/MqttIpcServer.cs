using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Protocol;
using SaltyEmu.Communication.Serializers;
using SaltyEmu.Communication.Utils;

namespace SaltyEmu.Communication.Communicators
{
    public abstract class MqttIpcServer<T> : IIpcServer where T : class
    {
        private readonly Logger Log = Logger.GetLogger<T>();
        private readonly IManagedMqttClient _client;
        private readonly IIpcSerializer _serializer;
        private readonly IPacketContainerFactory _packetFactory;
        private readonly IIpcRequestHandler _requestHandler;
        private readonly string _responseTopic;

        private readonly RabbitMqConfiguration _configuration;

        protected MqttIpcServer(RabbitMqConfiguration config, IIpcSerializer serializer, IIpcRequestHandler requestHandler, string requestTopic, string responseTopic)
        {
            _configuration = config;
            _requestHandler = requestHandler;
            _client = new MqttFactory().CreateManagedMqttClient();
            _client.SubscribeAsync(requestTopic);
            _responseTopic = responseTopic;

            _serializer = serializer;
            _packetFactory = new PacketContainerFactory();
        }

        public async Task InitializeAsync(string clientName)
        {
            ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId(clientName)
                    .WithTcpServer(_configuration.Address)
                    .Build())
                .Build();
            _client.ApplicationMessageReceived += (sender, args) => OnMessage(args.ClientId, args.ApplicationMessage);
            await _client.StartAsync(options);
        }

        private void OnMessage(string clientId, MqttApplicationMessage message)
        {
            var container = _serializer.Deserialize<PacketContainer>(message.Payload);
            object packet = JsonConvert.DeserializeObject(container.Content, container.Type);

            if (!(packet is BaseRequest request))
            {
                return;
            }

            OnRequest(request, container.Type);
        }

        public void OnRequest(BaseRequest request, Type type)
        {
            request.Server = this;
            _requestHandler.Handle(request, type);
        }


        public async Task ResponseAsync<T>(T response) where T : IIpcResponse
        {
            await SendAsync(_packetFactory.ToPacket<T>(response));
        }

        private async Task SendAsync(PacketContainer container)
        {
            await _client.PublishAsync(builder => builder
                .WithPayload(_serializer.Serialize(container))
                .WithTopic(_responseTopic));
        }
    }
}