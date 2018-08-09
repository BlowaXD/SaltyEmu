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
            var movableComponent = entity.GetComponent<MovableComponent>();
            if (movableComponent.Waypoints.Count == 0)
            {
                Random random = new Random();
                Position<short> dest = null;
                while (dest == null)
                {
                    short x = (short)random.Next(0, _map.Width),
                        y = (short)random.Next(0, _map.Height);
                    if (_map.IsWalkable(x, y))
                    {
                        dest = new Position<short> { X = x, Y = y };
                    }
                }
                foreach (var pos in _pathfinder.FindPath(movableComponent.Actual, dest, _map))
                {
                    movableComponent.Waypoints.Enqueue(pos);
                }
            }
            
            ProcessMovement(entity, movableComponent);

            /*
            if (!(entity is IMovableEntity movableEntity))
            {
                return;
            }

            MovableComponent movable = movableEntity.Movable;

            if (movable.Waypoints.Count <= 0 || !movable.CanMove())
            {
                return;
            }

            movable.Actual = movable.Waypoints.Dequeue();
            Move(entity);
            */
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
            foreach (IEntity entityy in entity.EntityManager.Entities)
            {
                if (Match(entityy) && entityy is IPlayerEntity player)
                {
                    player.SendPacket(new MvPacket(entityy));
                }
            }

            if (entity is IPlayerEntity playerEntity)
            {
                playerEntity.SendPacket(new CondPacketBase(playerEntity));
            }
        }

        private void ProcessMovement(IEntity entity, MovableComponent movableComponent)
        {
            if (movableComponent.CanMove())
            {
                movableComponent.Actual = movableComponent.Waypoints.Dequeue();
                Move(entity);
            }
        }
    }
}