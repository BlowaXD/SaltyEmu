using System;
using System.Collections.Generic;
using System.Diagnostics;
using ChickenAPI.Game.Battle.Events;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Battle
{
    public class BuffEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(BattleEntityAddBuffEvent),
            typeof(BattleEntityRemoveBuffEvent),
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case BattleEntityAddBuffEvent buff:
                    break;
                case BattleEntityRemoveBuffEvent buff:
                    break;
            }
        }
    }
}