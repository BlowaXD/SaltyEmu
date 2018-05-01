using System;
using System.Collections.Generic;
using ChickenAPI.Packets;
using ChickenAPI.Session;

namespace NosSharp.World.Packets
{
    public class PacketHandler : IPacketHandler
    {
        private readonly Dictionary<Type, PacketHandlerMethodReference> _packetHandler = new Dictionary<Type, PacketHandlerMethodReference>();
        private readonly Dictionary<string, PacketHandlerMethodReference> _packetHandlerMethod = new Dictionary<string, PacketHandlerMethodReference>();


        public void Register(PacketHandlerMethodReference method)
        {
            _packetHandlerMethod.TryAdd(method.Identification, method);
            _packetHandler.TryAdd(method.PacketDefinitionParameterType, method);
        }

        public void Unregister(Type type)
        {
            _packetHandler.Remove(type, out PacketHandlerMethodReference method);
            _packetHandlerMethod.Remove(method.Identification);
        }

        public PacketHandlerMethodReference GetPacketHandlerMethodReference(string header) => !_packetHandlerMethod.TryGetValue(header, out PacketHandlerMethodReference reference) ? null : reference;

        public void Handle(APacket packet, ISession session, Type type)
        {
            if (!_packetHandler.TryGetValue(type, out PacketHandlerMethodReference methodReference))
            {
                return;
            }

            if (!session.HasSelectedCharacter && methodReference.NeedCharacter)
            {
                return;
            }

            //check for the correct authority
            if (session.IsAuthenticated && (byte)methodReference.Authority > (byte)session.Account.Authority)
            {
                return;
            }


            if (packet != null)
            {
                methodReference.HandlerMethod(packet, session);
            }

            methodReference.HandlerMethod(packet, session);
        }
    }
}