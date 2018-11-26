using System;
using System.Collections.Concurrent;
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
using SaltyEmu.Communication.Utils;

namespace SaltyEmu.Communication.Communicators
{
    public abstract class MqttIpcClient<TLogger> : IIpcClient where TLogger : class
    {
        private readonly Logger Log = Logger.GetLogger<TLogger>();
        private readonly IManagedMqttClient _client;
        private readonly IIpcSerializer _serializer;
        private readonly IPacketContainerFactory _packetFactory;
        private readonly IPendingRequestFactory _requestFactory;
        private readonly string _requestTopic;
        private readonly string _endPoint;
        private readonly ConcurrentDictionary<Guid, PendingRequest> _pendingRequests;

        protected MqttIpcClient(MqttClientConfigurationBuilder builder) : this(builder.Build())
        {
        }

        protected MqttIpcClient(MqttIpcClientConfiguration conf)
        {
            _pendingRequests = new ConcurrentDictionary<Guid, PendingRequest>();
            _packetFactory = new PacketContainerFactory();
            _requestFactory = new PendingRequestFactory();
            _client = new MqttFactory().CreateManagedMqttClient(new MqttNetLogger(conf.ClientName));
            _client.SubscribeAsync(conf.ResponseTopic);

            _serializer = conf.Serializer;
            _requestTopic = conf.RequestTopic;
        }

        public async Task InitializeAsync(string clientName)
        {
            ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId(clientName)
                    .WithTcpServer(_endPoint)
                    .Build())
                .Build();
            _client.ApplicationMessageReceived += (sender, args) => OnMessage(args.ClientId, args.ApplicationMessage);
            await _client.StartAsync(options);
        }

        private void OnMessage(string clientId, MqttApplicationMessage message)
        {
            var container = _serializer.Deserialize<PacketContainer>(message.Payload);
            object packet = JsonConvert.DeserializeObject(container.Content, container.Type);

            if (!(packet is BaseResponse response))
            {
                return;
            }

            if (!_pendingRequests.TryGetValue(response.RequestId, out PendingRequest request))
            {
                return;
            }

            request.Response.SetResult(response);
        }

        public async Task<T> RequestAsync<T>(IIpcRequest packet) where T : class, IIpcResponse
        {
            // add packet to requests
            PendingRequest request = _requestFactory.Create(packet);
            if (!_pendingRequests.TryAdd(packet.Id, request))
            {
                return null;
            }

            // create the packet container
            PacketContainer container = _packetFactory.ToPacket(packet.GetType(), packet);
            await SendAsync(container);

            IIpcResponse tmp = await request.Response.Task;
            return tmp as T;
        }

        private async Task SendAsync(PacketContainer container)
        {
            await _client.PublishAsync(builder => builder
                .WithPayload(_serializer.Serialize(container))
                .WithTopic(_requestTopic));
        }

        public async Task BroadcastAsync<T>(T packet) where T : IIpcPacket
        {
            PacketContainer container = _packetFactory.ToPacket(packet.GetType(), packet);
            await SendAsync(container);
        }
    }
}