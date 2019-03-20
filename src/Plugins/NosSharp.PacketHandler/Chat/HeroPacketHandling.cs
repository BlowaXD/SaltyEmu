using NW.Plugins.PacketHandling.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using ChickenAPI.Packets.Game.Server.Player;
using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Chat.Events;

namespace NW.Plugins.PacketHandling.Chat
{
    public class HeroPacketHandling : GenericGamePacketHandlerAsync<HeroPacket>
    {
        protected override async Task Handle(HeroPacket packet, IPlayerEntity player)
        {
            try
            {
                await player.EmitEventAsync(new ChatHeroEvent
                {
                    Message = packet.Message
                });
            }
            catch (Exception e)
            {
                Log.Error("[HERO_PACKET]", e);
                throw;
            }
        }
    }
}
