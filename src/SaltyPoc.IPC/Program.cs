using System;
using ChickenAPI.Core.IPC;
using SaltyEmu.IpcPlugin.Communicators;

namespace SaltyPoc.IPC
{
    class Program
    {
        static void Main(string[] args)
        {
            IIpcServer server = new RabbitMQServer();
            // start server
            // start client
            // request
            Console.ReadKey();
        }
    }
}