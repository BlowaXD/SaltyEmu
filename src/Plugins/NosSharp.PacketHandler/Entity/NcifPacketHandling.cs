using System;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Battle.Extensions;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Extensions.UserInterface;
using ChickenAPI.Packets.ClientPackets.Battle;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Entity
{
    public class NcifPacketHandling : GenericGamePacketHandlerAsync<NcifPacket>
    {
        public NcifPacketHandling(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(NcifPacket packet, IPlayerEntity player)
        {
            try
            {
                var entity = player.CurrentMap.GetEntity<IBattleEntity>(packet.TargetId, packet.Type);
                player.SetTarget(entity);
                await player.ActualizeUiTargetHpBar();
            }
            catch (Exception e)
            {
                Log.Error("[NCIF]", e);
            }
        }
    }
}