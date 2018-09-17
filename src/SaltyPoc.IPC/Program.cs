using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SaltyPoc.IPC.PacketExample;
using SaltyPoc.IPC.Protocol;

namespace SaltyPoc.IPC
{
    class Program
    {
        static readonly RabbitMqCommunicator communicator = new RabbitMqCommunicator();
        private const string RequestQueueName = "salty_requests";
        private const string ResponseQueueName = "salty_responses";
        private const string BroadcastQueueName = "salty_broadcast";

        static void Main(string[] args)
        {
            if (args.Any(s => s.Contains("--serv")))
            {
                var factory = new ConnectionFactory { HostName = "localhost" };
                using (IConnection connection = factory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(RequestQueueName, true, false, false, null);
                        channel.QueueDeclare(ResponseQueueName, true, false, false, null);
                        channel.QueueDeclare(BroadcastQueueName, true, false, false, null);

                        // consumer

                        var responseConsumer = new EventingBasicConsumer(channel);
                        responseConsumer.Received += OnMessage;
                        channel.BasicConsume(ResponseQueueName, true, responseConsumer);

                        var requestConsumer = new AsyncEventingBasicConsumer(channel);
                        requestConsumer.Received += (sender, @event) =>
                        {
                            Console.WriteLine($"{Encoding.UTF8.GetString(@event.Body)}");
                            return Task.CompletedTask;
                        };
                        channel.BasicConsume(RequestQueueName, true, requestConsumer);

                        var broadcastConsumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(BroadcastQueueName, true, broadcastConsumer);

                        Console.WriteLine("Waiting for messages...");
                        Console.WriteLine("Press ENTER to exit.");
                        Console.ReadLine();
                    }
                }
            }
            else
            {
                DoTheWork().Wait();
            }
        }

        private static void OnMessage(object sender, BasicDeliverEventArgs e)
        {
            string requestMessage = Encoding.UTF8.GetString(e.Body);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Message : " + requestMessage);
            Console.ResetColor();

            var tmp = JsonConvert.DeserializeObject<BaseRequest>(requestMessage);
            if (tmp is GetFamilyMembersName packet)
            {
                OnRequestReceived(packet);
            }
        }


        /// <summary>
        /// Example
        /// </summary>
        private static readonly Dictionary<long, List<string>> Families = new Dictionary<long, List<string>>
        {
            { 1, new List<string> { "SaltyChef", "Kraken", "Syl" } }
        };


        /// <summary>
        /// Example handler
        /// </summary>
        /// <param name="packet"></param>
        public static async void OnRequestReceived(GetFamilyMembersName packet)
        {
            if (!Families.TryGetValue(packet.FamilyId, out List<string> names))
            {
                Console.WriteLine("ERROR : FamilyId not found");
            }

            await packet.RespondAsync(new GetFamilyMembersNameResponse
            {
                RequestId = packet.Id,
                Names = names
            });
        }

        private static async Task DoTheWork()
        {
            GetFamilyMembersNameResponse result = await communicator.RequestAsync<GetFamilyMembersNameResponse>(new GetFamilyMembersName
            {
                FamilyId = 1,
                Id = Guid.NewGuid(),
                Type = typeof(GetFamilyMembersName),
            });
            Console.WriteLine(result.Names.Aggregate((s, s1) => s += " " + s1));
            Console.ReadKey();
        }
    }
}