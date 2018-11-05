using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Features.GuriHandling.Args;
using ChickenAPI.Game.Features.GuriHandling.Handling;

namespace ChickenAPI.Game.Features.GuriHandling
{
    public class GuriEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>();
        private readonly IGuriHandler _guriHandler = new Lazy<IGuriHandler>(() => ChickenContainer.Instance.Resolve<IGuriHandler>()).Value;

        public override void Execute(IEntity entity, ChickenEventArgs args)
        {
            switch (args)
            {
                case GuriEventArgs guriEvent:
                    _guriHandler.Handle(entity as IPlayerEntity, guriEvent);
                    break;
            }
        }
    }
}