using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Managers;
using ChickenAPI.Packets.Game.Client._NotYetSorted;
using ChickenAPI.Packets.Game.Server.Map;

namespace NosSharp.PacketHandler.Move
{
    public class PreqPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<PreqPacketHandling>();

        private static readonly IMapManager MapManager = new Lazy<IMapManager>(ChickenContainer.Instance.Resolve<IMapManager>).Value;

        public static void OnPreqPacket(PreqPacket packet, IPlayerEntity session)
        {
            var currentMap = MapManager.GetBaseMapLayer(session.Character.MapId);
            var currentPortal = currentMap.Map.GetPortalFromPosition(session.Movable.Actual.X, session.Movable.Actual.Y);

            if (currentPortal == null)
            {
                Log.Warn($"Cannot find a valid portal at {session.Movable.Actual.X}x{session.Movable.Actual.Y} (Map ID: {session.Character.MapId}.");
                return;
            }

            session.SendPacket(new MapoutPacket());
            currentMap.Broadcast(new OutPacket() { VisualType = VisualType.Character, VisualId = session.Character.Id });

            session.Movable.Actual.X = currentPortal.DestinationX;
            session.Movable.Actual.Y = currentPortal.DestinationY;

            var destinationMap = MapManager.GetBaseMapLayer(currentPortal.DestinationMapId);

            if (destinationMap == null)
            {
                Log.Warn($"Cannot find map with id: {currentPortal.DestinationMapId}.");
                return;
            }

            session.TransferEntity(destinationMap);
        }
    }
}