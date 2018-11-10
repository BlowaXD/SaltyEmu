using System;
using System.Collections.Generic;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Npc.Events;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Entities.Npc
{
    public class NpcEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(NpcDeathEvent)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case NpcDeathEvent death:
                    if (!(entity is INpcEntity npc))
                    {
                        return;
                    }
                    break;
            }
        }
    }
}