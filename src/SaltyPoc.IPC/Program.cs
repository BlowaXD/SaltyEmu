using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SaltyPoc.IPC
{
    internal class BaseRequest
    {
        public Guid Id { get; set; }

        public Type Type { get; set; }
        public string Content { get; set; }

        public TaskCompletionSource<BaseResponse> Response { get; set; }
    }

    internal class BaseResponse
    {
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
        public Type ResponseType { get; set; }
        public string Content { get; set; }
    }

    internal sealed class ExampleResponse : BaseResponse
    {
    }

    public interface IIpcPacket
    {
    }

    internal sealed class Communicator
    {
        private static readonly Dictionary<Guid, BaseRequest> _requests = new Dictionary<Guid, BaseRequest>();
        private static List<BaseResponse> _response = new List<BaseResponse>();

        public static async Task<T> RequestAsync<T>(IIpcPacket packet) where T : BaseResponse
        {
            return await RequestAsync<T>(new BaseRequest
            {
                Id = Guid.NewGuid(),
                Content = JsonConvert.SerializeObject(packet),
                Response = new TaskCompletionSource<BaseResponse>(),
                Type = typeof(T),
            });
        }

        private static Task<T> RequestAsync<T>(BaseRequest request) where T : BaseResponse
        {
            // todo rabbitmq implementation & packet serialization
            _requests.Add(request.Id, request);

            // send message
            string correlationId = request.Id.ToString();
            string message = request.Content;


            return request.Response.Task as Task<T>;
        }

        public static void OnMessage(object sender, BasicDeliverEventArgs e)
        {
            string correlationId = e.BasicProperties.CorrelationId;
            string message = Encoding.UTF8.GetString(e.Body);
            var tmp = JsonConvert.DeserializeObject<BaseResponse>(message);
            if (!_requests.TryGetValue(tmp.Id, out BaseRequest request))
            {
                return;
            }

            request.Response.SetResult(tmp);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DoTheWork().Wait();
        }

        private static async Task DoTheWork()
        {
            var request = new BaseRequest();
            await Communicator.RequestAsync<ExampleResponse>(request);
            Console.WriteLine("Hello World!");
        }
    }
}