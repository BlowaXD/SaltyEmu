using System;
using ChickenAPI.ECS.Systems;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Systems.Movable;
using ChickenAPI.Packets.Game.Client;
using ChickenAPI.Packets.Game.Server;
using ChickenAPI.Utils;

namespace NosSharp.PacketHandler.Move
{
    public class WalkPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<WalkPacketHandling>();

        public static void OnWalkPacket(WalkPacket packet, IPlayerEntity session)
        {
            if (session.Movable.Actual.X == packet.XCoordinate && session.Movable.Actual.Y == packet.YCoordinate)
            {
                return;
            }

            if (session.Movable.Speed < packet.Speed)
            {
                return;
            }

            session.Movable.Actual.X = packet.XCoordinate;
            session.Movable.Actual.Y = packet.YCoordinate;
            session.Movable.Speed = packet.Speed;

            session.EntityManager.Broadcast(new MvPacket(session));
            session.SendPacket(new CondPacketBase(session));
        }
    }
}