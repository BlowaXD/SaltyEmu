using System;
using System.Collections.Generic;
using ChickenAPI.Game.Battle.Events;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Battle
{
    public class BattleEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes { get; }
        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case ProcessHitRequestEvent processHitRequest:
                    break;
                case FillHitRequestEvent fillHitRequest:
                    break;
            }
        }
    }
}