using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Map;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Portals.Args;
using ChickenAPI.Packets.Game.Client.Movement;

namespace NosSharp.PacketHandler.Move
{
    public class PreqPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<PreqPacketHandling>();

        private static readonly IMapManager MapManager = new Lazy<IMapManager>(ChickenContainer.Instance.Resolve<IMapManager>).Value;

        public static void OnPreqPacket(PreqPacket packet, IPlayerEntity session)
        {
            if (!(session.CurrentMap is IMapLayer mapLayer))
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

            session.EmitEvent(new PortalTriggerEvent
            {
                Portal = currentPortal,
            });
        }
    }
}