using System;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.Logging;
using SaltyEmu.IpcPlugin.Communicators;
using SaltyEmu.IpcPlugin.Utils;
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
            IIpcServer server = new RabbitMqServer(GetHandler());
            _client = new RabbitMqClient();
            await Test();
            Console.ReadKey();
        }

        private static async Task Test()
        {
            var req = new TestRequestPacket();
            Log.Info("RequestPacket : " + req.Id);
            TestResponsePacket resp = await _client.RequestAsync<TestResponsePacket>(req);
            if (resp == null) return; // not handled correctly

            Log.Info("ResponsePacket : " + resp.Id);
            Log.Info("ResponsePacket : " + resp.RequestId);
            Log.Info("ResponsePacket : " + resp.RandomPropertyAsATry);
        }
    }
}