using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Packets;
using ChickenAPI.Packets;

namespace NosSharp.PacketHandler.Utils
{
    public static class PacketMethodGenerator
    {
        public static IEnumerable<CharacterScreenPacketHandler> GetCharacterScreenPacketHandlers()
        {
            List<CharacterScreenPacketHandler> references = new List<CharacterScreenPacketHandler>();
            // iterate thru each type in the given assembly
            foreach (Type handlerType in typeof(CharacterScreenPacketsHandling).Assembly.GetTypes())
            {
                // include PacketDefinition
                references.AddRange(handlerType.GetMethods()
                    .Where(x => x.GetParameters().FirstOrDefault()?.ParameterType.BaseType == typeof(PacketBase) && x.GetParameters().Any(c => c.ParameterType == typeof(ISession))).Select(
                        methodInfo =>
                            new CharacterScreenPacketHandler(DelegateBuilder.BuildDelegate<Action<IPacket, ISession>>(methodInfo),
                                methodInfo.GetParameters().FirstOrDefault()?.ParameterType)));
            }

            return references;
        }

        public static IEnumerable<GamePacketHandler> GetGamePacketHandlers()
        {
            List<GamePacketHandler> references = new List<GamePacketHandler>();
            foreach (Type handlerType in typeof(CharacterScreenPacketsHandling).Assembly.GetTypes())
            {
                references.AddRange(handlerType.GetMethods()
                    .Where(x => x.GetParameters().FirstOrDefault()?.ParameterType.BaseType == typeof(PacketBase) && x.GetParameters().Any(c => c.ParameterType == typeof(IPlayerEntity))).Select(
                        methodInfo =>
                            new GamePacketHandler(DelegateBuilder.BuildDelegate<Action<IPacket, IPlayerEntity>>(methodInfo),
                                methodInfo.GetParameters().FirstOrDefault()?.ParameterType)));
            }

            return references;
        }
    }
}