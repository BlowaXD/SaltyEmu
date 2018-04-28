using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Packets;
using ChickenAPI.Session;

namespace NosSharp.PacketHandler.Utils
{
    public static class PacketMethodGenerator
    {
        public static IEnumerable<PacketHandlerMethodReference> GetHandlerMethodReference()
        {
            List<PacketHandlerMethodReference> references = new List<PacketHandlerMethodReference>();
            // iterate thru each type in the given assembly
            foreach (Type handlerType in typeof(CharacterScreenPacketHandler).Assembly.GetTypes())
            {
                // include PacketDefinition
                references.AddRange(handlerType.GetMethods()
                    .Where(x => x.GetParameters().FirstOrDefault()?.ParameterType.BaseType == typeof(APacket) && x.GetParameters().Any(c => c.ParameterType == typeof(ISession))).Select(methodInfo =>
                        new PacketHandlerMethodReference(DelegateBuilder.BuildDelegate<Action<APacket, ISession>>(methodInfo), handlerType,
                            methodInfo.GetParameters().FirstOrDefault()?.ParameterType)));
            }

            return references;
        }
    }
}