using System;
using System.Collections.Generic;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Features.Groups.Args;

namespace ChickenAPI.Game.Features.Groups
{
    public class GroupEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>();
        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
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
        }

        private void JoinGroup(IEntity entity, GroupJoinEventArgs groupJoin)
        {
            throw new NotImplementedException();
        }
    }
}