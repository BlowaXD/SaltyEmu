using System;
using System.Linq.Expressions;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.ECS.Systems;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Groups
{
    public class GroupSystem : SystemBase
    {
        public GroupSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        /// <summary>
        ///     3 ticks per second
        /// </summary>
        protected override double RefreshRate => 3;

        protected override Expression<Func<IEntity, bool>> Filter
        {
            get { return entity => entity.HasComponent<GroupComponent>(); }
        }

        protected override void Execute(IEntity entity)
        {
            if (!(entity is IPlayerEntity player))
            {
                return;
            }

            player.SendPackets(player.GeneratePstPacket());
        }
    }
}