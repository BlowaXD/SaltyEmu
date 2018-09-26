using System;
using Autofac;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Features.NpcDialog.Handlers;

namespace ChickenAPI.Game.Features.NpcDialog
{
    public class NpcDialogEventHandler : EventHandlerBase
    {
        private static readonly Logger Log = Logger.GetLogger<NpcDialogEventArgs>();
        private static readonly INpcDialogHandler NpcDialogHandler = new Lazy<INpcDialogHandler>(() => ChickenContainer.Instance.Resolve<INpcDialogHandler>()).Value;

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