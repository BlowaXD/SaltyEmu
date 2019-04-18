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
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Protocol;
using SaltyEmu.Communication.Utils;

namespace SaltyEmu.Communication.Communicators
{
    public abstract class RabbitMqClient : IDisposable, IRpcClient
    {
        private static readonly ILogger Log;
        private readonly RabbitMqConfiguration _configuration;
        private readonly string _requestQueueName;
        private readonly string _responseQueueName;
        private readonly string _broadcastQueueName;
        private const string ExchangeName = ""; // default exchange
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly IPacketContainerFactory _packetFactory;
        private readonly IPendingRequestFactory _requestFactory;

        private readonly ConcurrentDictionary<Guid, PendingRequest> _pendingRequests;

        protected RabbitMqClient(RabbitMqConfiguration config)
        {
            _configuration = config;
            var factory = new ConnectionFactory { HostName = _configuration.Address, Password = _configuration.Password, Port = _configuration.Port };

            _requestQueueName = _configuration.RequestQueueName;
            _responseQueueName = _configuration.ResponseQueueName;
            _broadcastQueueName = _configuration.BroadcastQueueName;

            _packetFactory = new PacketContainerFactory();
            _requestFactory = new PendingRequestFactory();

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(_requestQueueName, true, false, false, null);
            _channel.QueueDeclare(_responseQueueName, true, false, false, null);
            _channel.QueueDeclare(_broadcastQueueName, true, false, false, null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnMessage;

            _channel.BasicConsume(_responseQueueName, true, consumer);

            _pendingRequests = new ConcurrentDictionary<Guid, PendingRequest>();
            Log.Info("IPC Client launched !");
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }

        public async Task<T> RequestAsync<T>(ISyncRpcRequest packet) where T : class, ISyncRpcResponse
        {
            // add packet to requests
            PendingRequest request = _requestFactory.Create(packet);
            if (!_pendingRequests.TryAdd(packet.Id, request))
            {
                return null;
            }

            // create the packet container
            PacketContainer container = _packetFactory.ToPacket(packet.GetType(), packet);

            Publish(container, _requestQueueName);

            ISyncRpcResponse tmp = await request.Response.Task;
            return tmp as T;
        }

        public Task BroadcastAsync<T>(T packet) where T : IAsyncRpcRequest
        {
            return Task.Run(() =>
            {
                PacketContainer tmp = _packetFactory.ToPacket(packet);
                Publish(tmp, _broadcastQueueName);
            });
        }

        private void OnMessage(object sender, BasicDeliverEventArgs e)
        {
            string requestMessage = Encoding.UTF8.GetString(e.Body);
            var container = JsonConvert.DeserializeObject<PacketContainer>(requestMessage);
            object response = JsonConvert.DeserializeObject(container.Content, container.Type);

            if (!(response is BaseResponse baseResponse))
            {
                return;
            }

            if (!_pendingRequests.TryGetValue(baseResponse.RequestId, out PendingRequest request))
            {
                return;
            }

            request.Response.SetResult(baseResponse);
        }

        private void Publish(PacketContainer container, string queueName)
        {
            Log.Info($"[PACKET_SENT] {container.Type}");
            byte[] messageBytes = Encoding.UTF8.GetBytes(container.ToString());
            IBasicProperties props = _channel.CreateBasicProperties();
            props.ReplyTo = queueName;
            _channel.BasicPublish(ExchangeName, queueName, props, messageBytes);
        }
    }
}