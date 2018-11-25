using System;
using System.Collections.Generic;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Groups.Args;

namespace ChickenAPI.Game.Groups
{
    public class GroupEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(GroupJoinEvent),
            typeof(GroupInvitationSendEvent),
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case GroupJoinEvent groupJoin:
                    JoinGroup(entity, groupJoin);
                    break;
                case GroupInvitationSendEvent groupInvit:
                    GroupInvit(entity, groupInvit);
                    break;
            }
        }

        private void GroupInvit(IEntity entity, GroupInvitationSendEvent groupInvitationSend)
        {

        }

        private void JoinGroup(IEntity entity, GroupJoinEvent groupJoin)
        {
            throw new NotImplementedException();
        }
    }
}