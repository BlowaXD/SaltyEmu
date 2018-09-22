using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SaltyPoc.IPC
{
    public class RabbitMqCommunicator : IDisposable, IIpcClient
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
            _channel.QueueDeclare(BroadcastQueueName, true, false, false, null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnMessage;

            _channel.BasicConsume(RequestQueueName, true, consumer);
            _channel.BasicConsume(ResponseQueueName, true, consumer);
            _channel.BasicConsume(BroadcastQueueName, true, consumer);

            _pendingRequests = new ConcurrentDictionary<Guid, BaseRequest>();
        }

        public async Task<T> RequestAsync<T>(IIpcRequest packet) where T : IIpcResponse
        {
            string tmp = JsonConvert.SerializeObject(packet);
            return await RequestAsync<T>(new BaseRequest
            {
                Id = packet.Id == Guid.Empty ? Guid.NewGuid() : packet.Id,
                Content = tmp,
                Type = typeof(T),
                Response = new TaskCompletionSource<BaseResponse>(),
            });
        }

        public Task BroadcastAsync<T>(T packet) where T : IIpcPacket => throw new NotImplementedException();

        public Task SendAsync(IIpcPacket packet)
        {
            if (packet.Id == Guid.Empty)
            {
                packet.Id = Guid.NewGuid();
            }

            string tmp = JsonConvert.SerializeObject(packet);
            Publish(tmp, BroadcastQueueName);
            return Task.CompletedTask;
        }

        public Task<T> RespondAsync<T>(T packet) where T : IIpcResponse
        {
            Publish(JsonConvert.SerializeObject(packet), ResponseQueueName);
            return (Task<T>)Task.CompletedTask;
        }

        private async Task<T> RequestAsync<T>(BaseRequest request) where T : IIpcResponse
        {
            // todo rabbitmq implementation & packet serialization
            _pendingRequests.TryAdd(request.Id, request);

            // send message
            string correlationId = request.Id.ToString();
            string message = request.Content;
            Reply(message, correlationId);

            BaseResponse tmp = await request.Response.Task;


            return JsonConvert.DeserializeObject<T>(tmp.Content);
        }

        private void OnMessage(object sender, BasicDeliverEventArgs e)
        {
            string requestMessage = Encoding.UTF8.GetString(e.Body);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[CLIENT][RECV][" + e.BasicProperties.ReplyTo + "] :" + requestMessage);
            Console.ResetColor();
            switch (e.BasicProperties.ReplyTo)
            {
                case ResponseQueueName:
                    OnResponsePacket(JsonConvert.DeserializeObject<BaseResponse>(requestMessage));
                    break;
                case RequestQueueName:
                    OnRequestPacket(JsonConvert.DeserializeObject<BaseRequest>(requestMessage));
                    break;
            }
        }

        private void OnPacket(IIpcPacket deserializeObject)
        {
            // handle in handler
        }

        private void OnResponsePacket(IIpcResponse deserializeObject)
        {
            if (!(deserializeObject is BaseResponse response))
            {
                return;
            }
            
            BaseRequest tmp = _pendingRequests[response.RequestId];
            tmp?.Response.SetResult(response);
        }

        private void OnRequestPacket(IIpcRequest deserializeObject)
        {
            if (!(deserializeObject is BaseRequest request))
            {
                return;
            }

            request.Communicator = this;
        }


        private void Reply(string message, string correlationId)
        {
            IBasicProperties props = _channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = ResponseQueueName;

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(ExchangeName, RequestQueueName, props, messageBytes);
        }

        private void Publish(string message, string queueName)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
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