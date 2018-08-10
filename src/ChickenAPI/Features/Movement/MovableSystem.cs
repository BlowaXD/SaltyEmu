using System;
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
        protected override short RefreshRate => 1;

        public MovableSystem(IEntityManager entityManager, IMap map) : base(entityManager)
        {
            _map = map;
            _pathfinder = Container.Instance.Resolve<IPathfinder>();
        }

        private readonly IMap _map;
        private readonly IPathfinder _pathfinder;

        protected override Expression<Func<IEntity, bool>> Filter => entity => entity.HasComponent<MovableComponent>();

        protected override void Execute(IEntity entity)
        {
            const int range = 10;
            var movableComponent = entity.GetComponent<MovableComponent>();
            if (movableComponent.Waypoints.Count == 0 && entity.Type != EntityType.Player)
            {
                var random = new Random();
                Position<short> dest = null;
                while (dest == null)
                {
                    short x = (short)random.Next(movableComponent.Actual.X - range <= 0 ? 1 : movableComponent.Actual.X - range,
                        movableComponent.Actual.X - range >= _map.Width ? _map.Width - 1 : movableComponent.Actual.X - range);
                    short y = (short)random.Next(movableComponent.Actual.Y - _map.Height <= 0 ? 1 : movableComponent.Actual.Y - range,
                        movableComponent.Actual.Y - range >= _map.Height ? _map.Height - 1 : movableComponent.Actual.Y - range);
                    if (_map.IsWalkable(x, y))
                    {
                        dest = new Position<short> { X = x, Y = y };
                    }
                }

                foreach (Position<short> pos in _pathfinder.FindPath(movableComponent.Actual, dest, _map))
                {
                    movableComponent.Waypoints.Enqueue(pos);
                }
            }

            ProcessMovement(entity, movableComponent);
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

        private static void Move(IEntity entity)
        {
            var packet = new MvPacket(entity);
            if (entity.EntityManager is IMapLayer mapLayer)
            {
                mapLayer.Broadcast(packet);
            }

            if (entity is IPlayerEntity playerEntity)
            {
                playerEntity.SendPacket(new CondPacketBase(playerEntity));
            }
        }

        private void ProcessMovement(IEntity entity, MovableComponent movableComponent)
        {
            if (!movableComponent.CanMove() || movableComponent.Waypoints.Count <= 0)
            {
                return;
            }

            movableComponent.Actual = movableComponent.Waypoints.Dequeue();
            Move(entity);
        }
    }
}