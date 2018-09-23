using System;
using Autofac;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.GuriHandling.Args;
using ChickenAPI.Game.Features.GuriHandling.Handling;

namespace ChickenAPI.Game.Features.GuriHandling
{
    public class GuriEventHandler : EventHandlerBase
    {
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