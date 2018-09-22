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
        private static IIpcServer _server;
        private static IIpcClient _client;

        static void Main(string[] args)
        {
            Logger.Initialize();
            _server = new RabbitMqServer();
            _client = new RabbitMqClient();

            Test();
            Console.ReadKey();
        }

        static async Task Test()
        {
            TestResponsePacket tmp = await _client.RequestAsync<TestResponsePacket>(new TestRequestPacket());
        }
    }
}