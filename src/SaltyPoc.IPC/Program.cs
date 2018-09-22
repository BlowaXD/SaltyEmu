using System;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.Logging;
using SaltyEmu.IpcPlugin.Communicators;
using SaltyPoc.IPC.Packets;

namespace SaltyPoc.IPC
{
    class Program
    {
        private static readonly Logger Log = Logger.GetLogger<Program>();
        private static IIpcServer _server;
        private static IIpcClient _client;

        internal static async Task Main(string[] args)
        {
            Logger.Initialize();
            _server = new RabbitMqServer();
            _client = new RabbitMqClient();

            await Test();
            Console.ReadKey();
        }

        private static async Task Test()
        {
            var req = new TestRequestPacket();
            Log.Info("RequestPacket : " + req.Id);
            TestResponsePacket resp = await _client.RequestAsync<TestResponsePacket>(req);
            Log.Info("ResponsePacket : " + resp.Id);
            Log.Info("ResponsePacket : " + resp.RequestId);
            Log.Info("ResponsePacket : " + resp.Name);
            Log.Info("ResponsePacket : " + resp.Popopopo);
        }
    }
}