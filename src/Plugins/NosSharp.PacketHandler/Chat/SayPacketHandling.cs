﻿using System;
using System.Threading.Tasks;
using ChickenAPI.Game.Chat.Events;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client.Chat;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Chat
{
    public class SayPacketHandling : GenericGamePacketHandlerAsync<ClientSayPacket>
    {
        protected override async Task Handle(ClientSayPacket packet, IPlayerEntity player)
        {
            try
            {
                await player.EmitEventAsync(new ChatGeneralEvent
                {
                    Message = packet.Message
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