using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SaltyEmu.IpcPlugin.Protocol;

namespace SaltyEmu.IpcPlugin.Communicators
{
    public class RabbitMqServer : IIpcServer
    {
        private static readonly Logger Log = Logger.GetLogger<RabbitMqServer>();
        private readonly IConnection _connection;
        private readonly IModel _channel;


        private const string RequestQueueName = "salty_requests";
        private const string ResponseQueueName = "salty_responses";
        private const string BroadcastQueueName = "salty_broadcast";
        private const string ExchangeName = ""; // default exchange

        public RabbitMqServer()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(RequestQueueName, true, false, false, null);
            _channel.QueueDeclare(ResponseQueueName, true, false, false, null);
            _channel.QueueDeclare(BroadcastQueueName, true, false, false, null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnMessage;
            _channel.BasicConsume(RequestQueueName, true, consumer);
            _channel.BasicConsume(BroadcastQueueName, true, consumer);
            Log.Info("IPC Server launched !");
        }

        private void OnMessage(object sender, BasicDeliverEventArgs e)
        {
            string requestMessage = Encoding.UTF8.GetString(e.Body);
            var packet = JsonConvert.DeserializeObject<PacketContainer>(requestMessage);

            Log.Debug($"[OnMesssage] : " + requestMessage);

            switch (e.BasicProperties.ReplyTo)
            {
                case BroadcastQueueName:
                    break;
                case RequestQueueName:
                    object ipc = JsonConvert.DeserializeObject(packet.Content, packet.Type);
                    OnRequest(ipc as BaseRequest);
                    break;
            }
        }

        public void OnRequest(BaseRequest request)
        {
            Log.Info("[OnRequest]");
            request.Server = this;
        }


        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }

        public Task ResponseAsync<T>(T response) where T : IIpcResponse
        {
            _channel.BasicPublish(ExchangeName, ResponseQueueName);
            return Task.CompletedTask;
        }
    }
}