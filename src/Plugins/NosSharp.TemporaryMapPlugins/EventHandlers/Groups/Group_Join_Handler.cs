using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Groups.Args;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Group_Join_Handler : GenericEventPostProcessorBase<GroupJoinEvent>
    {
        protected override async Task Handle(GroupJoinEvent e, CancellationToken cancellation)
        {
        }
    }
}