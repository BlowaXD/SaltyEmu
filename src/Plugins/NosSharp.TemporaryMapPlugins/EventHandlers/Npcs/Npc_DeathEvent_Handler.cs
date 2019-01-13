using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Npc.Events;
using ChickenAPI.Game.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Npc_DeathEvent_Handler : GenericEventPostProcessorBase<NpcDeathEvent>
    {
        protected override Task Handle(NpcDeathEvent e, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }
}