using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Packets;
using ChickenAPI.Session;

namespace NosSharp.PacketHandler.Utils
{
    public static class PacketHandlerMethodReferenceGenerator
    {
        public static IEnumerable<PacketHandlerMethodReference> GetHandlerMethodReference()
        {
            List<PacketHandlerMethodReference> references = new List<PacketHandlerMethodReference>();
            List<IPacketHandlerMethodContainer> tmp = typeof(CharacterScreenPacketHandler).Assembly.GetInstancesOfImplementingTypes<IPacketHandlerMethodContainer>().ToList();
            // iterate thru each type in the given assembly
            foreach (IPacketHandlerMethodContainer handlerType in tmp)
            {
                var handler = (IPacketHandlerMethodContainer)Activator.CreateInstance(handlerType.GetType());

                // include PacketDefinition
                references.AddRange(handlerType.GetType().GetMethods().Where(x => x.GetParameters().FirstOrDefault()?.ParameterType.BaseType == typeof(APacket)).Select(methodInfo =>
                    new PacketHandlerMethodReference(DelegateBuilder.BuildDelegate<Action<APacket, ISession>>(methodInfo), handler, methodInfo.GetParameters().FirstOrDefault()?.ParameterType)));
            }

            return references;
        }
    }
}