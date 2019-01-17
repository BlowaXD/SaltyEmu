using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Entities.Events;
using ChickenAPI.Game.Entities.Npc.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Inventory;
using ChickenAPI.Game.Managers;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class ReqInfoEventHandler : GenericEventPostProcessorBase<ReqInfoEvent>
    {
        private readonly IPlayerManager _playerManager;
        private readonly INpcMonsterService _npcMonsterService;
        
        public ReqInfoEventHandler(IPlayerManager playerManager, INpcMonsterService npcMonsterService)
        {
            _playerManager = playerManager;
            _npcMonsterService = npcMonsterService;
        }

        public Task SendInfoFromMonster(IPlayerEntity player, ReqInfoEvent e)
        {
            NpcMonsterDto target = _npcMonsterService.GetById(e.TargetId);
            if (target == null)
            {
                return Task.CompletedTask;
            }

            //todo: use i18n to send the right npc name!
            return player?.SendPacketAsync(target.GenerateEInfoPacket());
        }

        public Task SendInfoFromPlayer(IPlayerEntity player, ReqInfoEvent e)
        {
            IPlayerEntity target = _playerManager.GetPlayerByCharacterId(e.TargetId);
            if (target == null)
            {
                return Task.CompletedTask;
            }

            return player?.SendPacketAsync(target.GenerateTcInfo());
        }

        public Task SendInfoFromItem(IPlayerEntity player, ReqInfoEvent e)
        {
            Log.Warn("SendInfoFromItem is unhandled yet. (SaltyEmu.BasicPlugin.EventHandlers/ReqInfoEventHandler L58)");
            return Task.CompletedTask;
        }

        protected override Task Handle(ReqInfoEvent e, CancellationToken cancellation)
        {
            Log.Debug($"Type of sender : {e.Sender.GetType()}");

            switch (e.ReqType)
            {
                case ReqInfoType.ItemInfo:
                    return SendInfoFromItem(e.Sender as IPlayerEntity, e);
                case ReqInfoType.NpcInfo:
                    return SendInfoFromMonster(e.Sender as IPlayerEntity, e);
                default:
                    return SendInfoFromPlayer(e.Sender as IPlayerEntity, e);
            }
        }
    }
}