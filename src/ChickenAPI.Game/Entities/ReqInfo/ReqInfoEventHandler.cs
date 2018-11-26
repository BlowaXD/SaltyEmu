using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.ReqInfo.Events;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Player.Extension;

namespace ChickenAPI.Game.Entities.ReqInfo
{
    public class ReqInfoEventHandler : EventHandlerBase
    {
        private static readonly IPlayerManager PlayerManager = new Lazy<IPlayerManager>(() => ChickenContainer.Instance.Resolve<IPlayerManager>()).Value;

        private static readonly Logger Log = Logger.GetLogger<ReqInfoEventHandler>();

        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(ReqInfoEvent)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case ReqInfoEvent aa:
                    switch (aa.ReqType)
                    {
                        case ReqInfoType.ItemInfo:
                            SenfInfoFromItem(entity as IInventoriedEntity, aa);
                            break;

                        case ReqInfoType.MateInfo:
                            break;

                        case ReqInfoType.NpcInfo:
                            SendInfoFromMonster(entity as INpcMonsterEntity, aa);
                            break;

                        default:
                            SendInfoFromPlayer(entity as IPlayerEntity, aa);
                            break;
                    }

                    break;
            }
        }

        public static void SendInfoFromPlayer(IPlayerEntity player, ReqInfoEvent e)
        {
            player.SendPacket(player.CurrentMap.GetPlayerById(e.TargetVNum).GenerateReqInfo());
        }

        public void SendInfoFromMonster(INpcMonsterEntity npc, ReqInfoEvent e)
        {
            Log.Info($"cc je verif le Npc");
        }

        public void SenfInfoFromItem(IInventoriedEntity item, ReqInfoEvent e)
        {
            Log.Info($"cc je verif l'item");
        }

        //public void SenInfoFromMate()
    }
}