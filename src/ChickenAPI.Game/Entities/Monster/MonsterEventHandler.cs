using System;
using System.Collections.Generic;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Monster.Events;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Entities.Monster
{
    public class MonsterEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(MonsterDeathEvent)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case MonsterDeathEvent death:
                    if (!(entity is IMonsterEntity monster))
                    {
                        return;
                    }
                    // Clear Buff/Debuff
                    // Set respawn
                    // Generate rewards if killer is player or mate
                    break;
            }
        }
    }
}