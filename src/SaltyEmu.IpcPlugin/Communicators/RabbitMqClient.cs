using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SaltyEmu.IpcPlugin.Protocol;

namespace SaltyEmu.IpcPlugin.Communicators
{
    public class RabbitMqClient : IDisposable, IIpcClient
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly ConcurrentDictionary<Guid, PendingRequest> _pendingRequests;
        private readonly IPacketContainerFactory _packetFactory;
        private readonly IPendingRequestFactory _requestFactory;

        private const string RequestQueueName = "salty_requests";
        private const string ResponseQueueName = "salty_responses";
        private const string BroadcastQueueName = "salty_broadcast";
        private const string ExchangeName = ""; // default exchange

        public RabbitMqClient()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _packetFactory = new PacketContainerFactory();
            _requestFactory = new PendingRequestFactory();

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(RequestQueueName, true, false, false, null);
            _channel.QueueDeclare(ResponseQueueName, true, false, false, null);
            _channel.QueueDeclare(BroadcastQueueName, true, false, false, null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnMessage;

            _channel.BasicConsume(ResponseQueueName, true, consumer);
            _channel.BasicConsume(BroadcastQueueName, true, consumer);

            _pendingRequests = new ConcurrentDictionary<Guid, PendingRequest>();
        }

        private PacketContainer ToPacket<T>(IIpcPacket packet)
        {
            return ToPacket(typeof(T), packet);
        }

        private PacketContainer ToPacket(Type type, IIpcPacket packet)
        {
            return _packetFactory.Create(type, JsonConvert.SerializeObject(packet));
        }

        public Task<T> RequestAsync<T>(IIpcRequest packet) where T : class, IIpcResponse
        {
            // add packet to requests
            PendingRequest request = _requestFactory.Create(packet);
            if (!_pendingRequests.TryAdd(packet.Id, request))
            {
                return null;
            }

            // create the packet container
            PacketContainer container = ToPacket(packet.GetType(), packet);

            Publish(container, RequestQueueName);

            return request.Response.Task as Task<T>;
        }

        public Task BroadcastAsync<T>(T packet) where T : IIpcPacket
        {
            return Task.Run(() =>
            {
                PacketContainer tmp = ToPacket<T>(packet);
                Publish(tmp, BroadcastQueueName);
            });
        }

        private void OnMessage(object sender, BasicDeliverEventArgs e)
        {
            string requestMessage = Encoding.UTF8.GetString(e.Body);
            var container = JsonConvert.DeserializeObject<PacketContainer>(requestMessage);
        }

        private void Publish(PacketContainer container, string queueName)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(container.ToString());
            IBasicProperties props = _channel.CreateBasicProperties();
            props.CorrelationId = "test";
            props.ReplyTo = queueName;
            _channel.BasicPublish(ExchangeName, queueName, props, messageBytes);
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}