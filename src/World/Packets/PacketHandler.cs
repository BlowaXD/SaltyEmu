using System;
using System.Collections.Generic;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.PacketHandling;
using ChickenAPI.Game.Packets;
using ChickenAPI.Packets;

namespace World.Packets
{
    public class PacketHandler : IPacketHandler
    {
        private static readonly Logger Log = Logger.GetLogger<PacketHandler>();
        private readonly Dictionary<string, CharacterScreenPacketHandler> _characterScreenByHeader = new Dictionary<string, CharacterScreenPacketHandler>();
        private readonly Dictionary<Type, CharacterScreenPacketHandler> _characterScreenHandlersByType = new Dictionary<Type, CharacterScreenPacketHandler>();
        private readonly Dictionary<string, GamePacketHandler> _gameHandlerByHeader = new Dictionary<string, GamePacketHandler>();

        private readonly Dictionary<Type, GamePacketHandler> _gameHandlersByType = new Dictionary<Type, GamePacketHandler>();


        public void Register(CharacterScreenPacketHandler method)
        {
            _characterScreenByHeader.TryAdd(method.Identification, method);
            _characterScreenHandlersByType.TryAdd(method.PacketType, method);
        }

        public void Register(GamePacketHandler handler)
        {
            _gameHandlersByType.TryAdd(handler.PacketType, handler);
            _gameHandlerByHeader.TryAdd(handler.PacketHeader.Identification, handler);
        }

        public void UnregisterGameHandler(string header)
        {
            throw new NotImplementedException();
        }

        public CharacterScreenPacketHandler GetCharacterScreenPacketHandler(string header) =>
            !_characterScreenByHeader.TryGetValue(header, out CharacterScreenPacketHandler handler) ? null : handler;

        public GamePacketHandler GetGamePacketHandler(string header) =>
            !_gameHandlerByHeader.TryGetValue(header, out GamePacketHandler handler) ? null : handler;


        public void UnregisterCharacterScreenHandler(Type packetType)
        {
            _characterScreenHandlersByType.Remove(packetType, out CharacterScreenPacketHandler method);
            _characterScreenByHeader.Remove(method.Identification);
        }

        public void UnregisterCharacterScreenHandler(string header)
        {
            _characterScreenByHeader.Remove(header, out CharacterScreenPacketHandler method);
            _characterScreenHandlersByType.Remove(method.PacketType);
        }

        public void UnregisterGameHandler(Type packetType)
        {
            _gameHandlersByType.Remove(packetType, out GamePacketHandler handler);
            _gameHandlerByHeader.Remove(handler.Identification);
        }

        public void Handle((IPacket, ISession) handlingInfo)
        {
            if (handlingInfo.Item1 == null)
            {
                Log.Warn($"[HANDLE][CHARACTERSCREEN] PacketNull by {handlingInfo.Item2.Ip}");
                return;
            }

            if (handlingInfo.Item2.Player != null)
            {
                // can't handle CharacterScreen packet while ingame
                Log.Warn($"[HANDLE][CHARACTERSCREEN] {handlingInfo.Item2.Ip} tries to use CharacterScreen packets while ingame");
                return;
            }

            if (!_characterScreenByHeader.TryGetValue(handlingInfo.Item1.Header, out CharacterScreenPacketHandler handler))
            {
                return;
            }

            handler.Handler(handlingInfo.Item1, handlingInfo.Item2);
        }

        public void Handle((IPacket, IPlayerEntity) handlingInfo)
        {
            if (handlingInfo.Item1 == null)
            {
                Log.Warn("[HANDLE][GAME] Wrong packet");
                return;
            }

            if (!_gameHandlerByHeader.TryGetValue(handlingInfo.Item1.Header, out GamePacketHandler handler))
            {
                return;
            }

            if (handler.Authority > handlingInfo.Item2.Session.Account.Authority)
            {
                Log.Warn($"[HANDLE][GAME] {handlingInfo.Item2.Session.Account.Name} Tries to use forbidden packets");
                return;
            }

            handler.HandlerMethod(handlingInfo.Item1, handlingInfo.Item2);
        }
    }
}