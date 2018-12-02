using System;
using System.Collections.Generic;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.PacketHandling;
using ChickenAPI.Packets;

namespace SaltyEmu.BasicPlugin
{
    public class PacketHandler : IPacketHandler
    {
        private static readonly Logger Log = Logger.GetLogger<PacketHandler>();

        /// <summary>
        /// character screen
        /// </summary>
        private readonly Dictionary<string, CharacterScreenPacketHandler> _characterScreenByHeader = new Dictionary<string, CharacterScreenPacketHandler>();

        private readonly Dictionary<Type, CharacterScreenPacketHandler> _characterScreenHandlersByType = new Dictionary<Type, CharacterScreenPacketHandler>();

        /// <summary>
        /// game
        /// </summary>
        private readonly Dictionary<string, GamePacketHandler> _gameHandlerByHeader = new Dictionary<string, GamePacketHandler>();

        private readonly Dictionary<Type, GamePacketHandler> _gameHandlersByType = new Dictionary<Type, GamePacketHandler>();


        public void Register(CharacterScreenPacketHandler method)
        {
            if (!_characterScreenByHeader.ContainsKey(method.Identification))
            {
                _characterScreenByHeader.Add(method.Identification, method);
            }

            if (!_characterScreenHandlersByType.ContainsKey(method.PacketType))
            {
                _characterScreenHandlersByType.Add(method.PacketType, method);
            }
        }

        public void Register(GamePacketHandler handler)
        {
            if (!_gameHandlerByHeader.ContainsKey(handler.Identification))
            {
                _gameHandlerByHeader.Add(handler.Identification, handler);
            }

            if (!_gameHandlersByType.ContainsKey(handler.PacketType))
            {
                _gameHandlersByType.Add(handler.PacketType, handler);
            }
        }

        public void UnregisterGameHandler(string header)
        {
            if (!_gameHandlerByHeader.TryGetValue(header, out GamePacketHandler method))
            {
                return;
            }

            _gameHandlerByHeader.Remove(header);
            _gameHandlersByType.Remove(method.PacketType);
        }

        public CharacterScreenPacketHandler GetCharacterScreenPacketHandler(string header) =>
            !_characterScreenByHeader.TryGetValue(header, out CharacterScreenPacketHandler handler) ? null : handler;

        public GamePacketHandler GetGamePacketHandler(string header) =>
            !_gameHandlerByHeader.TryGetValue(header, out GamePacketHandler handler) ? null : handler;


        public void UnregisterCharacterScreenHandler(Type packetType)
        {
            if (!_characterScreenHandlersByType.TryGetValue(packetType, out CharacterScreenPacketHandler method))
            {
                return;
            }

            _characterScreenHandlersByType.Remove(packetType);
            _characterScreenByHeader.Remove(method.Identification);
        }

        public void UnregisterCharacterScreenHandler(string header)
        {
            if (!_characterScreenByHeader.TryGetValue(header, out CharacterScreenPacketHandler method))
            {
                return;
            }

            _characterScreenByHeader.Remove(header);
            _characterScreenHandlersByType.Remove(method.PacketType);
        }

        public void UnregisterGameHandler(Type packetType)
        {
            if (!_gameHandlersByType.TryGetValue(packetType, out GamePacketHandler method))
            {
                return;
            }

            _gameHandlersByType.Remove(packetType);
            _gameHandlerByHeader.Remove(method.Identification);
        }

        public void Handle((IPacket, ISession) handlingInfo)
        {
            (IPacket packet, ISession session) = handlingInfo;
            if (packet == null)
            {
                Log.Warn($"[HANDLE][CHARACTERSCREEN] PacketNull by {session.Ip}");
                return;
            }

            if (session.Player != null)
            {
                // can't handle CharacterScreen packet while ingame
                Log.Warn($"[HANDLE][CHARACTERSCREEN] {session.Ip} tries to use CharacterScreen packets while ingame");
                return;
            }

            if (!_characterScreenByHeader.TryGetValue(packet.Header, out CharacterScreenPacketHandler handler))
            {
                return;
            }


            try
            {
                handler.Handler(packet, session);
            }
            catch (Exception e)
            {
                Log.Error("[CHARACTER_SCREEN_HANDLING]", e);
                throw;
            }
        }

        public void Handle((IPacket, IPlayerEntity) handlingInfo)
        {
            (IPacket packet, IPlayerEntity playerEntity) = handlingInfo;
            if (packet == null)
            {
                Log.Warn("[HANDLE][GAME] Wrong packet");
                return;
            }

            if (!_gameHandlerByHeader.TryGetValue(packet.Header, out GamePacketHandler handler))
            {
                return;
            }

            if (handler.Authority > playerEntity.Session.Account.Authority)
            {
                Log.Warn($"[HANDLE][GAME] {playerEntity.Session.Account.Name} Tries to use forbidden packets");
                return;
            }

            try
            {
                handler.HandlerMethod(packet, playerEntity);
            }
            catch (Exception e)
            {
                Log.Error("[GAME_HANDLING]", e);
                throw;
            }
        }
    }
}