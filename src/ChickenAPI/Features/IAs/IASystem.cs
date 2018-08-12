using System;
using System.Linq.Expressions;
using Autofac;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.ECS.Systems.Args;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Features.Movement;
using ChickenAPI.Game.Maps;

namespace ChickenAPI.Game.Features.IAs
{
    public class IASystem : NotifiableSystemBase
    {
        private readonly IMap _map;
        private readonly IPathfinder _pathfinder;
        private readonly Random _random = new Random();
        private readonly Random _randomY = new Random();

        public IASystem(IEntityManager entityManager, IMap map) : base(entityManager)
        {
            _map = map;
            _pathfinder = Container.Instance.Resolve<IPathfinder>();
        }

        protected override double RefreshRate => 0.45;

        protected override Expression<Func<IEntity, bool>> Filter => entity => MovableFilter(entity);

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

        protected override void Execute(IEntity entity)
        {
            int i = 0;
            var movableComponent = entity.GetComponent<MovableComponent>();
            if (movableComponent.Waypoints != null && movableComponent.Waypoints.Length != 0 || entity.Type == EntityType.Player)
            {
                return;
            }

            var random = new Random();
            if (random.Next(0, 100) < 35)
            {
                // wait max 2500 millisecs before having a new movement
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

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            switch (e)
            {
                case UpdateCacheEventArgs update:
                    UpdateCacheRequest = true;
                    break;
            }
        }
    }
}