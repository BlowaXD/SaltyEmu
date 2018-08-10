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
        protected override short RefreshRate => 10;

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
            if (entity.Type == EntityType.Player)
            {
                return;
            }

            const int range = 10;
            var i = 0;
            var movableComponent = entity.GetComponent<MovableComponent>();
            if (movableComponent.Waypoints.Count == 0 && entity.Type != EntityType.Player)
            {
                var random = new Random();
                Position<short> dest = null;
                while (dest == null && i < 5)
                {
                    short x = (short)random.Next(0, _map.Width);
                    short y = (short)random.Next(0, _map.Height);
                    if (_map.IsWalkable(x, y))
                    {
                        dest = new Position<short> { X = x, Y = y };
                    }

                    i++;
                }

                if (dest == null)
                {
                    return;
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

        private void Move(IEntity entity)
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