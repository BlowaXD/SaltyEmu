using System;
using ChickenAPI.Packets;
using ChickenAPI.Plugin;
using ChickenAPI.Utils;
using NosSharp.PacketHandler.Utils;

namespace NosSharp.PacketHandler
{
    public class PacketHandlerPlugin : IPlugin
    {
        public string Name => "Nos#-PacketHandler";

        public void OnDisable()
        {
            
        }

        public void OnEnable()
        {
            var packetHandler =  DependencyContainer.Instance.Get<IPacketHandler>();
            Console.WriteLine($"{packetHandler.GetType()}");
            foreach (PacketHandlerMethodReference handler in PacketMethodGenerator.GetHandlerMethodReference())
            {
                Console.WriteLine($"[PACKET] : {handler.Identification} Loaded !");
                packetHandler.Register(handler);
            }
        }

        public void OnLoad()
        {
            
        }
    }
}