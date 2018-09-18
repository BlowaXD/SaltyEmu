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
        private static RabbitMqCommunicator communicator;
        private const string RequestQueueName = "salty_requests";
        private const string ResponseQueueName = "salty_responses";
        private const string BroadcastQueueName = "salty_broadcast";
        private static IModel Channel;

        static void StartServer()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    Channel = channel;
                    channel.QueueDeclare(RequestQueueName, true, false, false, null);
                    channel.QueueDeclare(BroadcastQueueName, true, false, false, null);

                    // consumer

                    var responseConsumer = new EventingBasicConsumer(channel);
                    responseConsumer.Received += OnMessage;
                    channel.BasicConsume(RequestQueueName, true, responseConsumer);

                    var broadcastConsumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume(BroadcastQueueName, true, broadcastConsumer);

                    Console.WriteLine("Waiting for messages...");
                    Console.WriteLine("Press ENTER to exit.");
                    Console.ReadLine();
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Any(s => s.Contains("--serv")))
            {
                StartServer();
            }
            else
            {
                communicator = new RabbitMqCommunicator();
                DoTheWork().Wait();
            }
        }

        private static void OnMessage(object sender, BasicDeliverEventArgs e)
        {
            string requestMessage = Encoding.UTF8.GetString(e.Body);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[SERV][RECV] : " + requestMessage);
            Console.ResetColor();

            var tmp = JsonConvert.DeserializeObject<BaseRequest>(requestMessage);
            object pack = JsonConvert.DeserializeObject(requestMessage, tmp.Type);
            if (tmp.Type == typeof(GetFamilyMembersName))
            {
                OnRequestReceived(pack as GetFamilyMembersName);
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
        public static void OnRequestReceived(GetFamilyMembersName packet)
        {
            if (!Families.TryGetValue(packet.FamilyId, out List<string> names))
            {
                Console.WriteLine("ERROR : FamilyId not found");
            }

            string tmp = JsonConvert.SerializeObject(new GetFamilyMembersNameResponse
            {
                Id = Guid.NewGuid(),
                RequestId = packet.Id,
                Names = names
            });
            byte[] messageBytes = Encoding.UTF8.GetBytes(tmp);
            IBasicProperties props = Channel.CreateBasicProperties();
            props.CorrelationId = packet.Id.ToString();
            props.ReplyTo = ResponseQueueName;
            Channel.BasicPublish("", ResponseQueueName, props, messageBytes);
        }

        private static async Task DoTheWork()
        {
            await communicator.SendAsync(new GetFamilyMembersName
            {
                FamilyId = 2
            });
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