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
            foreach (PacketHandlerMethodReference handler in PacketHandlerMethodReferenceGenerator.GetHandlerMethodReference())
            {
                Console.WriteLine($"{handler.PacketHeader} Loaded !");
                packetHandler.Register(handler);
            }
        }

        public void OnLoad()
        {
            
        }
    }
}