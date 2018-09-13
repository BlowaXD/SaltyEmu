using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SaltyPoc.IPC.Protocol;

namespace SaltyPoc.IPC
{
    public class RabbitMqCommunicator : IDisposable, IIpcCommunicator
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly ConcurrentDictionary<Guid, BaseRequest> _pendingRequests;

        private const string RequestQueueName = "salty_requests";
        private const string ResponseQueueName = "salty_responses";
        private const string BroadcastQueueName = "salty_broadcast";
        private const string ExchangeName = ""; // default exchange

        public RabbitMqCommunicator()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(RequestQueueName, true, false, false, null);
            _channel.QueueDeclare(ResponseQueueName, true, false, false, null);
            _channel.QueueDeclare(BroadcastQueueName, true);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Registered += OnRegistered;
            consumer.Received += OnMessage;
            _channel.BasicConsume(ResponseQueueName, true, consumer);

            _pendingRequests = new ConcurrentDictionary<Guid, BaseRequest>();
        }

        private void OnRegistered(object sender, ConsumerEventArgs e)
        {
        }

        public async Task<T> RequestAsync<T>(IIpcPacket packet) where T : BaseResponse
        {
            return await RequestAsync<T>(new BaseRequest
            {
                Id = Guid.NewGuid(),
                Content = JsonConvert.SerializeObject(packet),
                Response = new TaskCompletionSource<BaseResponse>(),
                Type = typeof(T),
            });
        }

        public Task SendAsync(IIpcPacket packet)
        {
            string tmp = JsonConvert.SerializeObject(packet);
            Publish(tmp);
            return Task.CompletedTask;
        }

        private Task<T> RequestAsync<T>(BaseRequest request) where T : BaseResponse
        {
            // todo rabbitmq implementation & packet serialization
            _pendingRequests.TryAdd(request.Id, request);

            // send message
            string correlationId = request.Id.ToString();
            string message = request.Content;
            Reply(message, correlationId);


            return request.Response.Task as Task<T>;
        }

        private static void OnMessage(object sender, BasicDeliverEventArgs e)
        {
            string requestMessage = Encoding.UTF8.GetString(e.Body);
            var request = JsonConvert.DeserializeObject<BaseRequest>(requestMessage);
            string correlationId = e.BasicProperties.CorrelationId;
            string responseQueueName = e.BasicProperties.ReplyTo;

            // handle the packet received
        }

        private void Reply(string message, string correlationId)
        {
            IBasicProperties props = _channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = ResponseQueueName;

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(ExchangeName, RequestQueueName, props, messageBytes);
        }

        private void Publish(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(ExchangeName, BroadcastQueueName, body: messageBytes);
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}