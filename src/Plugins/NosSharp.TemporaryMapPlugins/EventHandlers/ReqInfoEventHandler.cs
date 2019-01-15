using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.ReqInfo.Events;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Player.Extension;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class ReqInfoEventHandler : GenericEventPostProcessorBase<ReqInfoEvent>
    {
        private readonly IPlayerManager _playerManager;

        private static readonly Logger Log = Logger.GetLogger<ReqInfoEventHandler>();

        public ReqInfoEventHandler(IPlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public static void SendInfoFromMonster(INpcMonsterEntity npc, ReqInfoEvent e)
        {
            Log.Info($"cc je verif le Npc");
        }

        public void SendInfoFromPlayer(IPlayerEntity player, ReqInfoEvent e)
        {
            IPlayerEntity target = _playerManager.GetPlayerByCharacterId(e.TargetVNum);
            if (target == null)
            {
                return;
            }

            player?.SendPacket(target.GenerateReqInfo());
        }

        public void SenfInfoFromItem(IInventoriedEntity item, ReqInfoEvent e)
        {
            Log.Info($"cc je verif l'item");
        }

        protected override async Task Handle(ReqInfoEvent e, CancellationToken cancellation)
        {
            switch (e.ReqType)
            {
                case ReqInfoType.ItemInfo:
                    SenfInfoFromItem(e.Sender as IInventoriedEntity, e);
                    break;

                case ReqInfoType.MateInfo:
                    break;

                case ReqInfoType.NpcInfo:
                    SendInfoFromMonster(e.Sender as INpcMonsterEntity, e);
                    break;

                default:
                    SendInfoFromPlayer(e.Sender as IPlayerEntity, e);
                    break;
            }
        }
    }
}