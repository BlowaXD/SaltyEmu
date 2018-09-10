using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SaltyPoc.IPC
{
    internal class BaseRequest
    {
        public Guid Id { get; set; }

        public Type Type { get; set; }
        public string Content { get; set; }
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

    internal sealed class Communicator
    {
        private static List<BaseRequest> _requests = new List<BaseRequest>();
        private static List<BaseResponse> _response = new List<BaseResponse>();

        public static Task<T> RequestAsync<T>(BaseRequest request) where T : BaseResponse
        {
            // todo rabbitmq implementation & packet serialization
            DateTime timeout = DateTime.UtcNow.AddSeconds(5);
            return Task.Run(() =>
            {
                while (timeout > DateTime.UtcNow)
                {
                    BaseResponse tmp = _response.FirstOrDefault(s => s.RequestId == request.Id);
                    if (tmp == null || tmp.ResponseType != typeof(T))
                    {
                        continue;
                    }

                    return JsonConvert.DeserializeObject<T>(tmp.Content);
                }

                return null;
            });
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