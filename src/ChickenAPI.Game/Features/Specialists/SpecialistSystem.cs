using System;
using System.Linq.Expressions;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;

namespace ChickenAPI.Game.Features.Specialists
{
    public class SpecialistSystem : SystemBase
    {
        public SpecialistSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        // all 10 seconds
        protected override double RefreshRate => 0.1;
        protected override Expression<Func<IEntity, bool>> Filter => entity => entity.Type == EntityType.Player;

        protected override void Execute(IEntity entity)
        {
            if (!(entity is IPlayerEntity player))
            {
                return;
            }

            if (player.Sp == null)
            {
                return;
            }

            // remove points
            player.SendPacket(player.GenerateSpPacket());
        }
    }
}