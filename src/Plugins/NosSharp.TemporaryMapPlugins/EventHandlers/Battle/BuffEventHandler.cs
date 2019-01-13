using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Battle.Events;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers.Battle
{
    public class BattleEntity_AddBuff_Handler : GenericEventPostProcessorBase<BattleEntityAddBuffEvent>
    {
        protected override Task Handle(BattleEntityAddBuffEvent e, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }

    public class BattleEntity_RemoveBuff_Handler : GenericEventPostProcessorBase<BattleEntityRemoveBuffEvent>
    {
        protected override Task Handle(BattleEntityRemoveBuffEvent e, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }
}