using System;
using System.Linq.Expressions;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.ECS.Systems.Args;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Groups
{
    public class GroupSystem : NotifiableSystemBase
    {
        /// <summary>
        /// 3 ticks per second
        /// </summary>
        protected override short RefreshRate => 3;

        protected override Expression<Func<IEntity, bool>> Filter
        {
            get { return entity => entity.HasComponent<GroupComponent>(); }
        }

        public GroupSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        protected override void Execute(IEntity entity)
        {
            if (!(entity is IPlayerEntity player))
            {
                return;
            }
            // send group packet

            player.SendPackets(player.GeneratePstPacket());
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}