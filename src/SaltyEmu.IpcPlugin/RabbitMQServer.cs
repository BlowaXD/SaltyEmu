using System;
using System.Collections.Concurrent;
using System.Text;
using ChickenAPI.Core.IPC.Protocol;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SaltyPoc.IPC
{
    public class RabbitMQServer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly ConcurrentDictionary<Guid, BaseRequest> _pendingRequests;

        private const string RequestQueueName = "salty_requests";
        private const string ResponseQueueName = "salty_responses";
        private const string BroadcastQueueName = "salty_broadcast";
        private const string ExchangeName = ""; // default exchange

        public RabbitMQServer()
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
        }

        private void OnMessage(object sender, BasicDeliverEventArgs e)
        {
            string requestMessage = Encoding.UTF8.GetString(e.Body);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.ResetColor();
        }

        private void OnPacket(IIpcPacket deserializeObject)
        {
            // handle in handler
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}