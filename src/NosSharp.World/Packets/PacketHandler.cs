using System;
using System.Collections.Generic;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Network;
using ChickenAPI.Packets;

namespace NosSharp.World.Packets
{
    public class PacketHandler : IPacketHandler
    {
        private readonly Dictionary<Type, CharacterScreenPacketHandler> _characterScreenHandlersByType = new Dictionary<Type, CharacterScreenPacketHandler>();
        private readonly Dictionary<string, CharacterScreenPacketHandler> _characterScreenByHeader = new Dictionary<string, CharacterScreenPacketHandler>();

        private readonly Dictionary<Type, GamePacketHandler> _gameHandlersByType = new Dictionary<Type, GamePacketHandler>();
        private readonly Dictionary<string, GamePacketHandler> _gameHandlerByHeader = new Dictionary<string, GamePacketHandler>();


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
                return;
            }

            if (handlingInfo.Item2 != null)
            {
                // can't handle CharacterScreen packet while ingame
            }
            throw new NotImplementedException();
        }

        public void Handle((IPacket, IPlayerEntity) handlingInfo)
        {
            throw new NotImplementedException();
        }

        public void Handle((IPacket, ISession) session, Type type)
        {
            /*
            if (packet == null)
            {
                return;
            }

            if (session.Player != null)
            {
                Handle(packet, session.Player, type);
                return;
            }

            if (!_characterScreenHandlersByType.TryGetValue(type, out CharacterScreenPacketHandler methodReference))
            {
                return;
            }

            //check for the correct authority
            if (session.IsAuthenticated && (byte)methodReference.Authority > (byte)session.Account.Authority)
            {
                return;
            }

            methodReference.Handler(packet, session);
            */
        }

        public void Handle(IPacket packet, IPlayerEntity player, Type type)
        {
            /*
            if (packet == null)
            {
                return;
            }

            if (player == null)
            {
                return;
            }

            if (!_characterScreenHandlersByType.TryGetValue(type, out CharacterScreenPacketHandler methodReference))
            {
                return;
            }

            //check for the correct authority
            if ((byte)methodReference.Authority > (byte)player.Session.Account.Authority)
            {
                return;
            }

            // todo cleanup this
            methodReference.Handler(packet, player.Session);
            */
        }
    }
}