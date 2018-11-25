using System;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.Communication.Communicators
{
    public abstract class MqttCommunicator : IIpcClient
    {
        private readonly IManagedMqttClient _client;
        private readonly RabbitMqConfiguration _configuration;

        protected MqttCommunicator(RabbitMqConfiguration config, IIpcSerializer serializer)
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
            await _client.StartAsync(options);
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