using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Battle.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers.Battle
{
    public class BattleEntity_AddBuff_Handler : GenericEventPostProcessorBase<BattleEntityAddBuffEvent>
    {
        public BattleEntity_AddBuff_Handler(ILogger log) : base(log)
        {
        }

        protected override Task Handle(BattleEntityAddBuffEvent e, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }

    public class BattleEntity_RemoveBuff_Handler : GenericEventPostProcessorBase<BattleEntityRemoveBuffEvent>
    {
        public BattleEntity_RemoveBuff_Handler(ILogger log) : base(log)
        {
        }

        protected override Task Handle(BattleEntityRemoveBuffEvent e, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }
}