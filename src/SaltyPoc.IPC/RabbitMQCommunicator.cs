using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SaltyPoc.IPC.PacketExample;
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
            Publish(tmp, BroadcastQueueName);
            return Task.CompletedTask;
        }

        public Task<T> RespondAsync<T>(T packet) where T : IIpcResponse
        {
            Publish(JsonConvert.SerializeObject(packet), ResponseQueueName);
            return (Task<T>)Task.CompletedTask;
        }

        private Task<T> RequestAsync<T>(BaseRequest request) where T : IIpcResponse
        {
            // todo rabbitmq implementation & packet serialization
            _pendingRequests.TryAdd(request.Id, request);

            // send message
            string correlationId = request.Id.ToString();
            string message = request.Content;
            Reply(message, correlationId);


            return request.Response.Task as Task<T>;
        }

        private void OnMessage(object sender, BasicDeliverEventArgs e)
        {
            string requestMessage = Encoding.UTF8.GetString(e.Body);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Message : " + requestMessage);
            Console.ResetColor();
            switch (e.BasicProperties.ReplyTo)
            {
                case ResponseQueueName:
                    OnResponsePacket(JsonConvert.DeserializeObject<IIpcResponse>(requestMessage));
                    break;
                case RequestQueueName:
                    OnRequestPacket(JsonConvert.DeserializeObject<IIpcRequest>(requestMessage));
                    break;
                case BroadcastQueueName:
                    OnPacket(JsonConvert.DeserializeObject<IIpcPacket>(requestMessage));
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

            if (!_pendingRequests.TryRemove(response.RequestId, out BaseRequest request))
            {
                return;
            }

            request.Response.SetResult(response);
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
            _channel.BasicPublish(ExchangeName, queueName, body: messageBytes);
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}