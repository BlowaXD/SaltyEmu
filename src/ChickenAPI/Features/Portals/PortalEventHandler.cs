using System;
using Autofac;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Portals.Args;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Packets.Game.Server.Map;

namespace ChickenAPI.Game.Features.Portals
{
    public class PortalEventHandler : EventHandlerBase
    {
        private static readonly Logger Log = Logger.GetLogger<PortalEventHandler>();
        private static readonly IMapManager MapManager = new Lazy<IMapManager>(ChickenContainer.Instance.Resolve<IMapManager>).Value;
        public override void Execute(IEntity entity, ChickenEventArgs args)
        {
            switch (args)
            {
                case PortalTriggerEvent portalTriggerEvent:
                    TriggerPortalEventHandler(entity, portalTriggerEvent);
                    break;
            }
        }

        private static void TriggerPortalEventHandler(IEntity entity, PortalTriggerEvent args)
        {
            // todo check portal state
            if (!(entity.EntityManager is IMapLayer currentMap))
            {
                return;
            }
            if (!(entity is IPlayerEntity session))
            {
                return;
            }

            PortalDto currentPortal = args.Portal;

            session.SendPacket(new MapoutPacket());
            currentMap.Broadcast(session, session.GenerateOutPacket());

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