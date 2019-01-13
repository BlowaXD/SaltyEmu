using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.GuriHandling.Args;
using ChickenAPI.Game.GuriHandling.Handling;

namespace SaltyEmu.BasicPlugin.EventHandlers.Guri
{
    public class GuriEventHandler : GenericEventPostProcessorBase<GuriEvent>
    {
        private readonly IGuriHandler _guriHandler;

        public GuriEventHandler(IGuriHandler guriHandler)
        {
            _guriHandler = guriHandler;
        }

        protected override Task Handle(GuriEvent e, CancellationToken cancellation)
        {
            return Task.Run(() => _guriHandler.Handle(e.Sender as IPlayerEntity, e), cancellation);
        }
    }
}