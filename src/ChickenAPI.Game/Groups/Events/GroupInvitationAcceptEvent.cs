using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Groups.Args
{
    public class GroupInvitationAcceptEvent : GameEntityEvent
    {
        public GroupInvitDto Invitation { get; set; }
    }
}