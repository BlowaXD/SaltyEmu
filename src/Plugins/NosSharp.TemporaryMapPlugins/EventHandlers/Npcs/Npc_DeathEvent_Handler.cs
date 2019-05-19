using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Npc.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Npc_DeathEvent_Handler : GenericEventPostProcessorBase<NpcDeathEvent>
    {
        public Npc_DeathEvent_Handler(ILogger log) : base(log)
        {
        }

        protected override Task Handle(NpcDeathEvent e, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }
}