using System;
using System.Collections.Generic;
using ChickenAPI.Game.Battle.Events;
using ChickenAPI.Game.Battle.Hitting;
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
        }

        private void FillEffects(HitRequest hitRequest)
        {
        }

        private void SetupHitType(HitRequest hitRequest)
        {
        }
    }
}