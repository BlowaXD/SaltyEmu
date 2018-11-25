using System;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Utils;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Protocol;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.Communication.Communicators
{
    public abstract class MqttCommunicator : IIpcClient
    {
        protected event EventHandler<IIpcPacket> PacketReceived;


        private readonly IManagedMqttClient _client;
        private readonly IIpcSerializer<PacketContainer> _serializer;
        private readonly RabbitMqConfiguration _configuration;

        protected MqttCommunicator(RabbitMqConfiguration config, IIpcSerializer<PacketContainer> serializer)
        {
            _configuration = config;
            _client = new MqttFactory().CreateManagedMqttClient();
            _serializer = serializer;
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
            PacketContainer packet = _serializer.Deserialize(message.Payload);
        }

        public Task<T> RequestAsync<T>(IIpcRequest request) where T : class, IIpcResponse
        {
            return (Task<T>)Task.CompletedTask;
        }

        public Task BroadcastAsync<T>(T packet) where T : IIpcPacket
        {
            return Task.CompletedTask;
        }
    }
}