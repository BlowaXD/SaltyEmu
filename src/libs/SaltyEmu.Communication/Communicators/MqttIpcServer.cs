using System;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Protocol;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.Communication.Communicators
{
    public abstract class MqttIpcServer<TLogger> : IIpcServer where TLogger : class
    {
        private readonly Logger Log = Logger.GetLogger<TLogger>();
        private readonly IManagedMqttClient _client;
        private readonly IIpcSerializer _serializer;
        private readonly IPacketContainerFactory _packetFactory;
        private readonly IIpcRequestHandler _requestHandler;
        private readonly string _clientName;
        private readonly string _responseTopic;
        private readonly string _endPoint;

        protected MqttIpcServer(MqttServerConfigurationBuilder builder) : this(builder.Build())
        {
        }

        protected MqttIpcServer(MqttIpcServerConfiguration configuration)
        {
            _clientName = configuration.ClientName;
            _responseTopic = configuration.ResponseTopic;
            _serializer = configuration.Serializer;
            _requestHandler = configuration.Handler;
            _endPoint = configuration.EndPoint;


            _client = new MqttFactory().CreateManagedMqttClient(new MqttNetLogger(_clientName));

            foreach (string topic in configuration.SubscribingQueues)
            {
                Log.Info($"Subscribing topic : {topic}");
                _client.SubscribeAsync(topic);
            }

            _packetFactory = new PacketContainerFactory();
        }

        public async Task InitializeAsync()
        {
            ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId(_clientName)
                    .WithTcpServer(_endPoint)
                    .Build())
                .Build();
            _client.ApplicationMessageReceived += (sender, args) => OnMessage(args.ClientId, args.ApplicationMessage);
            await _client.StartAsync(options);
            _client.Connected += (sender, args) =>
            {
                Log.Info($"Connected to {_endPoint}");
                SendAsync(_packetFactory.Create<TLogger>("test")).ConfigureAwait(false).GetAwaiter().GetResult();
            };
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
#if DEBUG
            Log.Debug($"[RECEIVED] Packet [{request.Id}][{type}]");
#endif
            request.Server = this;
            _requestHandler.Handle(request, type);
        }


        public async Task ResponseAsync<T>(T response) where T : IIpcResponse
        {
            await SendAsync(_packetFactory.ToPacket<T>(response));
        }

        private async Task SendAsync(PacketContainer container)
        {
#if DEBUG
            Log.Debug($"[SENT] Packet [{container.Type}][{container.Content}]");
#endif
            await _client.PublishAsync(builder => builder
                .WithPayload(_serializer.Serialize(container))
                .WithTopic(_responseTopic));
        }
    }
}