using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets.Attributes;
using ChickenAPI.Packets.Interfaces;

namespace SaltyEmu.BasicPlugin.Implems
{
    public class BasicPacketPipelineAsync : IPacketPipelineAsync
    {
        private readonly Dictionary<string, Type> _packetTypesByHeader = new Dictionary<string, Type>();
        private readonly Dictionary<Type, IPacketProcessor> _packetProcessors = new Dictionary<Type, IPacketProcessor>();

        public Task RegisterAsync(IPacketProcessor processor, Type packetType)
        {
            if (_packetProcessors.ContainsKey(packetType))
            {
                return Task.CompletedTask;
            }

            _packetTypesByHeader.Add(packetType.GetCustomAttribute<PacketHeaderAttribute>().Identification, packetType);
            _packetProcessors[packetType] = processor;
            return Task.CompletedTask;
        }

        public Task UnregisterAsync(IPacketProcessor processor, Type packetType)
        {
            _packetProcessors.Remove(packetType);
            _packetTypesByHeader.Remove(packetType.GetCustomAttribute<PacketHeaderAttribute>().Identification);
            return Task.CompletedTask;
        }

        public Task Handle(IPacket packet, ISession session)
        {
            if (!_packetProcessors.TryGetValue(packet.GetType(), out IPacketProcessor processor))
            {
                return Task.CompletedTask;
            }

            return processor.Handle(packet, session);
        }

        public Type PacketTypeByHeader(string header)
        {
            return !_packetTypesByHeader.TryGetValue(header, out Type type) ? null : type;
        }
    }
}