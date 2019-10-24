using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Map;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Portals.Events;
using ChickenAPI.Game._ECS.Entities;

namespace SaltyEmu.BasicPlugin.EventHandlers.Portals
{
    public class PortalEventHandler : GenericEventPostProcessorBase<PortalTriggerEvent>
    {
        private readonly IMapManager _mapManager;


        public PortalEventHandler(ILogger log, IMapManager mapManager) : base(log) => _mapManager = mapManager;

        protected override Task Handle(PortalTriggerEvent e, CancellationToken cancellation)
        {
            // todo check portal state
            if (!(e.Sender is IPlayerEntity session))
            {
                return Task.CompletedTask;
            }

            PortalDto currentPortal = e.Portal;

            session.Position.X = currentPortal.DestinationX;
            session.Position.Y = currentPortal.DestinationY;
            IMapLayer destinationMap = _mapManager.GetBaseMapLayer(currentPortal.DestinationMapId);

            if (destinationMap == null)
            {
                Log.Warn($"Cannot find map with id: {currentPortal.DestinationMapId}.");
                return Task.CompletedTask;
            }

            session.TransferEntity(destinationMap);
            return Task.CompletedTask;
        }
    }
}