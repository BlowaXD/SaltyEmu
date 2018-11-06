using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Map;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Features.Portals.Args;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.PacketHandling.Extensions;
using ChickenAPI.Packets.Game.Server.Map;

namespace ChickenAPI.Game.Portals
{
    public class PortalEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>();
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
            if (!(entity is IPlayerEntity session))
            {
                return;
            }

            PortalDto currentPortal = args.Portal;

            session.SendPacket(new MapoutPacket());
            session.BroadcastExceptSender(session.GenerateOutPacket());

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