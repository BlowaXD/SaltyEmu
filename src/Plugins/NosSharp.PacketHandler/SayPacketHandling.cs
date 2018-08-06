﻿using System;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Chat;
using ChickenAPI.Game.Packets.Game.Client;

namespace NosSharp.PacketHandler
{
    public class SayPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<SayPacketHandling>();
        public static void OnSayPacket(ClientSayPacket packetBase, IPlayerEntity session)
        {
            try
            {
                session.EntityManager.NotifySystem<ChatSystem>(session, new PlayerChatEventArg
                {
                    Message = packetBase.Message,
                    SenderId = session.Session.CharacterId,
                    Sender = session,
                });
            }
            catch (Exception e)
            {
                Log.Error("[SAY_PACKET]", e);
                throw;
            }
        }
    }
}