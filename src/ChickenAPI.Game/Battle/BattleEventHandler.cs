using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Battle.Events;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Battle
{
    public class BattleEventHandler : EventHandlerBase
    {
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
                case ProcessHitRequestEvent processHitRequest:
                    // do the whole graphical & stats processing
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
            var algo = ChickenContainer.Instance.Resolve<IDamageAlgorithm>();
            hitRequest.Damages = algo.GenerateDamage(hitRequest);
        }

        private void FillEffects(HitRequest hitRequest)
        {
        }

        private void SetupHitType(HitRequest hitRequest)
        {
        }
    }
}