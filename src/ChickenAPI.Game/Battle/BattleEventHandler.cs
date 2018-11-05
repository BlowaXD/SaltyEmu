using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Battle.Events;
using ChickenAPI.Game.Battle.Extensions;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;
using ChickenAPI.Packets.Game.Server.Battle;

namespace ChickenAPI.Game.Battle
{
    public class BattleEventHandler : EventHandlerBase
    {
        private readonly Logger Log = Logger.GetLogger<BattleEventHandler>();
        private readonly IDamageAlgorithm _algo = new Lazy<IDamageAlgorithm>(() => ChickenContainer.Instance.Resolve<IDamageAlgorithm>()).Value;

        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(ProcessHitRequestEvent),
            typeof(FillHitRequestEvent)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            if (!(entity is IBattleEntity battleEntity))
            {
                return;
            }

            switch (e)
            {
                case ProcessHitRequestEvent processHitRequest: // do the whole graphical & stats processing
                    ProcessHitRequest(processHitRequest.HitRequest);
                    break;
                case FillHitRequestEvent fillHitRequest:
                    SetupHitType(fillHitRequest.HitRequest);
                    FillEffects(fillHitRequest.HitRequest);
                    DamageCalculation(fillHitRequest.HitRequest);
                    break;
            }
        }

        private void DamageCalculation(HitRequest hitRequest)
        {
            hitRequest.Damages = _algo.GenerateDamage(hitRequest);
        }

        private void FillEffects(HitRequest hitRequest)
        {
        }

        private void SetupHitType(HitRequest hitRequest)
        {
        }

        private void ProcessHitRequest(HitRequest hitRequest)
        {
            IBattleEntity target = hitRequest.Target;
            uint givenDamages = 0;
            List<SuPacket> packets = new List<SuPacket>();
            while (givenDamages != hitRequest.Damages && target.IsAlive)
            {
                ushort nextDamages = hitRequest.Damages - givenDamages > ushort.MaxValue ? ushort.MaxValue : (ushort)(hitRequest.Damages - givenDamages);
                givenDamages += nextDamages;
                if (target.Hp - nextDamages <= 0)
                {
                    target.Hp = 0;
                    // Generate Death
                    break;
                }

                target.Hp -= nextDamages;
                packets.Add(hitRequest.Sender.GenerateSuPacket(hitRequest, nextDamages));
            }

            hitRequest.Sender.CurrentMap.Broadcast<SuPacket>(packets);
            Log.Debug($"[{hitRequest.Sender.Type.ToString()}][{hitRequest.Sender.Id}] ATTACK -> [{hitRequest.Target.Type.ToString()}]({hitRequest.Target.Id}) : {givenDamages} damages");
            // apply buffs
            // apply debuffs
        }
    }
}