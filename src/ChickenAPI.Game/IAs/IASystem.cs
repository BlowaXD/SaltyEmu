using System;
using System.Linq.Expressions;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Core.Utils;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Movements;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game._ECS.Systems;

namespace ChickenAPI.Game.IAs
{
    public class IASystem : SystemBase
    {
        private readonly IMap _map;
        private readonly IPathfinder _pathfinder;
        private readonly IRandomGenerator _random;

        public IASystem(IEntityManager entityManager, IMap map) : base(entityManager)
        {
            _map = map;
            _pathfinder = ChickenContainer.Instance.Resolve<IPathfinder>();
            _random = ChickenContainer.Instance.Resolve<IRandomGenerator>();
        }

        protected override double RefreshRate => 0.45;

        protected override Expression<Func<IEntity, bool>> Filter => entity => MovableFilter(entity);

        private static bool MovableFilter(IEntity entity)
        {
            if (entity.Type == VisualType.Character)
            {
                return false;
            }


            return entity is IMovableEntity;
        }

        protected override void Execute(IEntity entity)
        {
            if (!(entity is IAiEntity mov))
            {
                return;
            }

            if (!mov.IsAlive)
            {
                return;
            }

            int i = 0;


            if (mov.Waypoints != null && mov.Waypoints.Length != 0 || entity.Type == VisualType.Character || mov.Speed == 0)
            {
                return;
            }

            if (_random.Next(0, 100) < 35)
            {
                // wait max 2500 millisecs before having a new movement
                mov.LastMove = DateTime.UtcNow.AddMilliseconds(_random.Next(2500));
                return;
            }

            Position<short> dest = null;
            while (dest == null && i < 25)
            {
                short xpoint = (short)_random.Next(0, 4);
                short ypoint = (short)_random.Next(0, 4);
                short firstX = mov.Position.X;
                short firstY = mov.Position.Y;
                dest = _map.GetFreePosition(firstX, firstY, xpoint, ypoint);

                i++;
            }

            if (dest == null)
            {
                return;
            }

            mov.Waypoints = _pathfinder.FindPath(mov.Position, dest, _map);
        }
    }
}