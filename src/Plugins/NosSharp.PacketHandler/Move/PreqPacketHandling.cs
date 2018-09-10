using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Portals;
using ChickenAPI.Game.Features.Portals.Args;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Maps;
using ChickenAPI.Packets.Game.Client._NotYetSorted;

namespace NosSharp.PacketHandler.Move
{
    public class PreqPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<PreqPacketHandling>();

        private static readonly IMapManager MapManager = new Lazy<IMapManager>(ChickenContainer.Instance.Resolve<IMapManager>).Value;

        public static void OnPreqPacket(PreqPacket packet, IPlayerEntity session)
        {
            if (!(session.EntityManager is IMapLayer mapLayer))
            {
                Log.Warn("Should be in a map");
                return;
            }

            IMapLayer currentMap = mapLayer;
            PortalDto currentPortal = currentMap.Map.GetPortalFromPosition(session.Movable.Actual.X, session.Movable.Actual.Y);

            if (currentPortal == null)
            {
                Log.Warn($"Cannot find a valid portal at {session.Movable.Actual.X}x{session.Movable.Actual.Y} (Map ID: {session.Character.MapId}.");
                return;
            }

            session.NotifyEventHandler<PortalEventHandler>(new PortalTriggerEvent
            {
                Portal = currentPortal,
            });
        }
    }
}