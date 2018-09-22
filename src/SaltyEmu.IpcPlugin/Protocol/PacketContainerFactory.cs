using System;
using ChickenAPI.Core.IPC.Protocol;
using Newtonsoft.Json;

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

        public PacketContainer ToPacket<T>(IIpcPacket packet)
        {
            return ToPacket(typeof(T), packet);
        }

        public PacketContainer ToPacket(Type type, IIpcPacket packet)
        {
            return Create(type, JsonConvert.SerializeObject(packet));
        }
    }
}