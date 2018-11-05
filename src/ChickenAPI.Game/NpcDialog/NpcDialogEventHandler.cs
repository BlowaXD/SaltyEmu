using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Features.NpcDialog.Handlers;

namespace ChickenAPI.Game.Features.NpcDialog
{
    public class NpcDialogEventHandler : EventHandlerBase
    {
        private static readonly Logger Log = Logger.GetLogger<NpcDialogEventArgs>();
        private static readonly INpcDialogHandler NpcDialogHandler = new Lazy<INpcDialogHandler>(() => ChickenContainer.Instance.Resolve<INpcDialogHandler>()).Value;

        public override ISet<Type> HandledTypes => new HashSet<Type>();
        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case NpcDialogEventArgs dialogevent:
                    NpcDialogHandler.Execute(entity as IPlayerEntity, dialogevent);
                    break;
            }
        }
    }
}