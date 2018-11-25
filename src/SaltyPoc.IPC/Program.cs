using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace SaltyPoc.IPC
{
    internal class Program
    {
        private static readonly Logger Log = Logger.GetLogger<Program>();


        internal static async Task Main(string[] args)
        {
            Logger.Initialize();
            // Setup and start a managed MQTT client.

            ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId("Client1")
                    .WithTcpServer("localhost")
                    .Build())
                .Build();

            IManagedMqttClient mqttClient = new MqttFactory().CreateManagedMqttClient();
            await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("families").Build());
            await mqttClient.StartAsync(options);


            mqttClient.Connected += (sender, eventArgs) =>
            {
                Log.Info("Connected !");
                Task.Run(() => { Send(mqttClient).Wait(); });
            };
            mqttClient.ApplicationMessageReceived += (sender, eventArgs) =>
            {
                Log.Info($"Message Received {eventArgs.ApplicationMessage.Topic} : {Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload)}");
            };
            mqttClient.ConnectingFailed += (sender, eventArgs) => { Log.Error("[CONNECTION_FAILED]", eventArgs.Exception); };


            // StartAsync returns immediately, as it starts a new thread using Task.Run, 
            // and so the calling thread needs to wait.
            Console.ReadLine();
        }

        private static async Task Send(IManagedMqttClient mqttClient)
        {
            DateTime tmp = DateTime.Now.AddMilliseconds(500);
            await mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic("families")
                .WithPayload(Encoding.UTF8.GetBytes("mabite")).Build());
            await Task.Delay(tmp - DateTime.Now);
            await Send(mqttClient);
        }
    }
}