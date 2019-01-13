using System;
using System.Threading.Tasks;
using ChickenAPI.Game.Battle.Extensions;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Extensions.UserInterface;
using ChickenAPI.Packets.Game.Client.Battle;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Entity
{
    public class NcifPacketHandling : GenericGamePacketHandlerAsync<NcifPacket>
    {
        protected override async Task Handle(NcifPacket packet, IPlayerEntity player)
        {
            try
            {
                var entity = player.CurrentMap.GetEntity<IBattleEntity>(packet.TargetId, packet.Type);
                player.SetTarget(entity);
                player.ActualizeUiTargetHpBar();
            }
            catch (Exception e)
            {
                Log.Error("[NCIF]", e);
            }
        }
    }
}