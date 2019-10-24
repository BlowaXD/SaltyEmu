using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Groups.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Group_Join_Handler : GenericEventPostProcessorBase<GroupJoinEvent>
    {
        public Group_Join_Handler(ILogger log) : base(log)
        {
        }

        protected override Task Handle(GroupJoinEvent e, CancellationToken cancellation)
        {
            // ?
            return Task.CompletedTask;
        }
    }
}