using System;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.NpcDialog;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using SaltyEmu.Communication.Utils;
using SaltyPoc.IPC.Packets;

namespace SaltyPoc.IPC
{
    internal class Program
    {
        private static readonly Logger Log = Logger.GetLogger<Program>();
        private static IIpcClient _client;

        internal static IIpcRequestHandler GetHandler()
        {
            IIpcRequestHandler requestHandler = new RequestHandler();
            requestHandler.Register<TestRequestPacket>(async packet =>
            {
                await packet.ReplyAsync(new TestResponsePacket());
                Log.Info("Replied to TestRequest");
            });
            return requestHandler;
        }

        internal static async Task Main(string[] args)
        {
            Logger.Initialize();
            // Setup and start a managed MQTT client.

            ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId("Client1")
                    .WithTcpServer("broker.hivemq.com")
                    .WithTls().Build())
                .Build();

            IManagedMqttClient mqttClient = new MqttFactory().CreateManagedMqttClient();
            await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("my/topic").Build());
            await mqttClient.StartAsync(options);

            // StartAsync returns immediately, as it starts a new thread using Task.Run, 
            // and so the calling thread needs to wait.
            Console.ReadLine();
        }

        private static async Task Test()
        {
            var req = new TestRequestPacket();
            Log.Info("RequestPacket : " + req.Id);
            TestResponsePacket resp = await _client.RequestAsync<TestResponsePacket>(req);
            if (resp == null)
            {
                return; // not handled correctly
            }

            Log.Info("ResponsePacket : " + resp.Id);
            Log.Info("ResponsePacket : " + resp.RequestId);
            Log.Info("ResponsePacket : " + resp.RandomPropertyAsATry);
        }
    }
}