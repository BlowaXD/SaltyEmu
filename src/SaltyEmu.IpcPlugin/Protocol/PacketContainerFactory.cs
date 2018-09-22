using System;

namespace SaltyEmu.IpcPlugin.Protocol
{
    public class PacketContainerFactory : IPacketContainerFactory
    {
        public PacketContainer Create<T>(string content) => Create(typeof(T), content);

        public PacketContainer Create(Type type, string content)
        {
            return new PacketContainer
            {
                Type = type,
                Content = content,
            };
        }
    }
}