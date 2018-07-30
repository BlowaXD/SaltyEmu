using System;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Systems.Chat;
using ChickenAPI.Packets.Game.Client;
using ChickenAPI.Packets.Game.Server;
using ChickenAPI.Utils;

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