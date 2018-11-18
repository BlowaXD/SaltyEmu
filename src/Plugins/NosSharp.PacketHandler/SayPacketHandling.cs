using System;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Chat.Args;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client.Chat;

namespace NosSharp.PacketHandler
{
    public class SayPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<SayPacketHandling>();

        public static void OnSayPacket(ClientSayPacket packetBase, IPlayerEntity session)
        {
            try
            {
                session.EmitEvent(new PlayerChatEventArg
                {
                    Message = packetBase.Message,
                    SenderId = session.Session.CharacterId,
                    Sender = session
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