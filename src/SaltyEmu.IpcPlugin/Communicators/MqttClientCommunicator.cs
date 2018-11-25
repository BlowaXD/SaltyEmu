using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Utils;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using Newtonsoft.Json;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Protocol;
using SaltyEmu.Communication.Serializers;
using SaltyEmu.Communication.Utils;

namespace SaltyEmu.Communication.Communicators
{
    public abstract class MqttClientCommunicator : IIpcClient
    {
        protected event EventHandler<IIpcPacket> PacketReceived;


        private readonly IManagedMqttClient _client;
        private readonly IIpcSerializer _serializer;
        private readonly IPacketContainerFactory _packetFactory;
        private readonly IPendingRequestFactory _requestFactory;

        private readonly RabbitMqConfiguration _configuration;
        private readonly ConcurrentDictionary<Guid, PendingRequest> _pendingRequests;

        protected MqttClientCommunicator(RabbitMqConfiguration config, IIpcSerializer serializer)
        {
            _configuration = config;
            _client = new MqttFactory().CreateManagedMqttClient();
            _serializer = serializer;
            _pendingRequests = new ConcurrentDictionary<Guid, PendingRequest>();
            _packetFactory = new PacketContainerFactory();
            _requestFactory = new PendingRequestFactory();
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
                .WithTopic("topic"));
        }

        public Task BroadcastAsync<T>(T packet) where T : IIpcPacket
        {
            return Task.CompletedTask;
        }
    }
}