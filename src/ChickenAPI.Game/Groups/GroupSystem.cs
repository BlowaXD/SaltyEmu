using System;
using System.Linq;
using System.Linq.Expressions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game._ECS.Systems;

namespace ChickenAPI.Game.Groups
{
    public class GroupSystem : SystemBase
    {
        public GroupSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        /// <summary>
        ///     3 ticks per second
        /// </summary>
        protected override double RefreshRate => 0.5;

        protected override Expression<Func<IEntity, bool>> Filter => entity => entity is IPlayerEntity;

        protected override void Execute(IEntity entity)
        {
            if (!(entity is IPlayerEntity player))
            {
                return;
            }

            if (player.HasGroup || player.ActualMates.Any())
            {
                player.ActualizeUiGroupStats();
            }
        }
    }
}