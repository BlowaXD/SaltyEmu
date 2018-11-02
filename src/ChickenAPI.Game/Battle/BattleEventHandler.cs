using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Battle.DataObjects;
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
            BattleComponent battleTarget = hitRequest.Target.Battle;
            uint damage = hitRequest.Damages;
            List<SuPacket> packets = new List<SuPacket>();
            while (damage > 0)
            {
                hitRequest.Damages = damage > ushort.MaxValue ? ushort.MaxValue : (ushort)damage;
                damage -= hitRequest.Damages;
                packets.Add(hitRequest.Sender.GenerateSuPacket(hitRequest));
                if (battleTarget.Hp - hitRequest.Damages <= 0)
                {
                    battleTarget.Hp = 0;
                    // Generate Death
                    break;
                }
                battleTarget.Hp -= (ushort)hitRequest.Damages;
            }

            if (hitRequest.Sender.CurrentMap is IMapLayer broadcastable)
            {
                broadcastable.Broadcast<SuPacket>(packets);
            }

            // apply buffs
            // apply debuffs
        }
    }
}