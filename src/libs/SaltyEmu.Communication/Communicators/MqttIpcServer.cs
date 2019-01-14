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
        private readonly Logger _log = Logger.GetLogger<TLogger>();
        private readonly IManagedMqttClient _client;
        private readonly IIpcSerializer _serializer;
        private readonly IPacketContainerFactory _packetFactory;
        private readonly string _clientName;
        private readonly string _responseTopic;
        private readonly string _endPoint;

        // handling
        protected readonly IIpcRequestHandler RequestHandler;

        protected MqttIpcServer(MqttServerConfigurationBuilder builder) : this(builder.Build())
        {
        }

        private MqttIpcServer(MqttIpcServerConfiguration configuration)
        {
            _clientName = configuration.ClientName;
            _responseTopic = configuration.ResponseTopic;
            _serializer = configuration.Serializer;
            RequestHandler = configuration.Handler;
            _endPoint = configuration.EndPoint;


            _client = new MqttFactory().CreateManagedMqttClient(new MqttNetLogger(_clientName));

            foreach (string topic in configuration.SubscribingQueues)
            {
                _log.Info($"[RPC] Waiting for requests from : {topic}...");
                _client.SubscribeAsync(topic);
            }

            _log.Info($"[RPC] Waiting for broadcasts from : {configuration.BroadcastTopic}...");
            _client.SubscribeAsync(configuration.BroadcastTopic);

            _packetFactory = new PacketContainerFactory();
        }

        protected async Task InitializeAsync()
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
            _client.Connected += (sender, args) => _log.Info($"[CONNECTED] {_clientName} is connected on MQTT Broker {_endPoint}");
            _client.Disconnected += (sender, args) => _log.Info($"[DISCONNECTED] {_clientName} has been disconnected on MQTT Broker {_endPoint}");
        }

        private void OnMessage(string clientId, MqttApplicationMessage message)
        {
            var container = _serializer.Deserialize<PacketContainer>(message.Payload);
            object packet = JsonConvert.DeserializeObject(container.Content, container.Type);

            switch (packet)
            {
                case BaseRequest request:
                    OnRequest(request, container.Type);
                    break;
                case BaseBroadcastedPacket broadcasted:
                    OnBroadcastPacket(broadcasted, container.Type);
                    break;
            }
        }

        public void OnBroadcastPacket(BaseBroadcastedPacket broadcasted, Type type)
        {
#if DEBUG
            _log.Debug($"[RECEIVED] Packet [{type}]");
            RequestHandler.HandleBroadcastAsync(broadcasted, type).ConfigureAwait(false).GetAwaiter().GetResult();
#endif
        }

        public void OnRequest(BaseRequest request, Type type)
        {
#if DEBUG
            _log.Debug($"[RECEIVED] Request [{request.Id}][{type}]");
#endif
            request.Server = this;
            RequestHandler.HandleAsync(request, type).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        public async Task RegisterRequestsAsync<T>(T packet) where T : IIpcRequest => throw new NotImplementedException();

        public async Task RegisterPacketsAsync<T>(T packet) where T : IIpcPacket => throw new NotImplementedException();

        public async Task ResponseAsync<T>(T response) where T : IIpcResponse
        {
            await SendAsync(_packetFactory.ToPacket<T>(response));
        }

        private async Task SendAsync(PacketContainer container)
        {
#if DEBUG
            _log.Debug($"[SENT] Packet [{container.Type}]");
#endif
            await _client.PublishAsync(builder => builder
                .WithPayload(_serializer.Serialize(container))
                .WithTopic(_responseTopic));
        }
    }
}