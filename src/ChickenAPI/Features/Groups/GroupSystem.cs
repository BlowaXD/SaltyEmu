using System;
using System.Linq.Expressions;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.ECS.Systems.Args;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Groups.Args;

namespace ChickenAPI.Game.Features.Groups
{
    public class GroupSystem : NotifiableSystemBase
    {
        /// <summary>
        /// 3 ticks per second
        /// </summary>
        protected override double RefreshRate => 3;

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

            player.SendPackets(player.GeneratePstPacket());
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            switch (e)
            {
                case UpdateCacheEventArgs update:
                    UpdateCacheRequest = true;
                    break;
                case GroupJoinEventArgs groupJoin:
                    JoinGroup(entity, groupJoin);
                    break;
                case GroupInvitEventArgs groupInvit:
                    GroupInvit(entity, groupInvit);
                    break;
            }
        }

        private void GroupInvit(IEntity entity, GroupInvitEventArgs groupInvit)
        {
            return;
        }

        private void JoinGroup(IEntity entity, GroupJoinEventArgs groupJoin)
        {
            throw new NotImplementedException();
        }
    }
}