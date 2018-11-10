using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Map;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Portals.Args;

namespace ChickenAPI.Game.Portals
{
    public class PortalEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(PortalTriggerEvent)
        };

        private static readonly Logger Log = Logger.GetLogger<PortalEventHandler>();
        private static readonly IMapManager MapManager = new Lazy<IMapManager>(ChickenContainer.Instance.Resolve<IMapManager>).Value;

        public override void Execute(IEntity entity, ChickenEventArgs args)
        {
            switch (args)
            {
                case PortalTriggerEvent portalTriggerEvent:
                    TriggerPortalEventHandler(portalTriggerEvent);
                    break;
            }
        }

        private static void TriggerPortalEventHandler(PortalTriggerEvent args)
        {
            // todo check portal state
            if (!(args.Sender is IPlayerEntity session))
            {
                return;
            }

            PortalDto currentPortal = args.Portal;

            session.Movable.Actual.X = currentPortal.DestinationX;
            session.Movable.Actual.Y = currentPortal.DestinationY;
            IMapLayer destinationMap = MapManager.GetBaseMapLayer(currentPortal.DestinationMapId);

            if (destinationMap == null)
            {
                Log.Warn($"Cannot find map with id: {currentPortal.DestinationMapId}.");
                return;
            }

            session.TransferEntity(destinationMap);
        }
    }
}