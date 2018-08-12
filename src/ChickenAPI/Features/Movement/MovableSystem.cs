using System;
using System.Linq;
using System.Linq.Expressions;
using Autofac;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.ECS.Systems.Args;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Features.Movement
{
    public class MovableSystem : NotifiableSystemBase
    {
        protected override short RefreshRate => 2;

        public MovableSystem(IEntityManager entityManager, IMap map) : base(entityManager)
        {
            _map = map;
            _pathfinder = Container.Instance.Resolve<IPathfinder>();
        }

        private readonly IMap _map;
        private readonly IPathfinder _pathfinder;
        private readonly Random _random = new Random();
        private readonly Random _randomY = new Random();

        private static bool MovableFilter(IEntity entity)
        {
            if (entity.Type == EntityType.Player)
            {
                return false;
            }

            var movable = entity.GetComponent<MovableComponent>();
            if (movable == null)
            {
                return false;
            }

            return movable.Speed != 0;
        }

        protected override Expression<Func<IEntity, bool>> Filter => entity => MovableFilter(entity);

        protected override void Execute(IEntity entity)
        {
            try
            {
                int i = 0;
                var movableComponent = entity.GetComponent<MovableComponent>();
                if ((movableComponent.Waypoints == null || movableComponent.Waypoints.Length == 0) && entity.Type != EntityType.Player)
                {
                    var random = new Random();
                    if (random.Next(0, 100) < 45)
                    {
                        movableComponent.LastMove = DateTime.UtcNow.AddMilliseconds(random.Next(2500));
                        return;
                    }

                    Position<short> dest = null;
                    while (dest == null && i < 25)
                    {
                        short xpoint = (short)_random.Next(0, 4);
                        short ypoint = (short)_randomY.Next(0, 4);
                        short firstX = movableComponent.Actual.X;
                        short firstY = movableComponent.Actual.Y;
                        dest = _map.GetFreePosition(firstX, firstY, xpoint, ypoint);

                        i++;
                    }

                    if (dest == null)
                    {
                        return;
                    }

                    movableComponent.Waypoints = _pathfinder.FindPath(movableComponent.Actual, dest, _map);
                }

                ProcessMovement(entity, movableComponent);
            }
            catch (Exception e)
            {
                Log.Error("[MOVABLE] UPDATE()", e);
            }
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            switch (e)
            {
                case UpdateCacheEventArgs update:
                    UpdateCacheRequest = true;
                    break;
            }
        }

        private void Move(IEntity entity)
        {
            try
            {
                var packet = new MvPacket(entity);
                if (EntityManager is IMapLayer mapLayer) // wtf ?
                {
                    mapLayer.Broadcast(packet);
                }

                if (entity is IPlayerEntity playerEntity)
                {
                    playerEntity.SendPacket(new CondPacketBase(playerEntity));
                }
            }
            catch (Exception e)
            {
                Log.Error("Move()", e);
            }
        }

        private void ProcessMovement(IEntity entity, MovableComponent movableComponent)
        {
            if (movableComponent.Waypoints.Length <= 0)
            {
                return;
            }

            if (movableComponent.Speed == 0)
            {
                return;
            }

            byte speedIndex = (byte)(movableComponent.Speed / 2 < 1 ? 1 : movableComponent.Speed / 2);
            int maxindex = movableComponent.Waypoints.Length > speedIndex ? speedIndex : movableComponent.Waypoints.Length;
            Position<short> newPos = movableComponent.Waypoints[maxindex - 1];

            if (!movableComponent.CanMove(newPos))
            {
                return;
            }

            movableComponent.Actual = movableComponent.Waypoints[maxindex - 1];
            movableComponent.Waypoints = movableComponent.Waypoints.Skip(maxindex).ToArray();
            Move(entity);
        }
    }
}