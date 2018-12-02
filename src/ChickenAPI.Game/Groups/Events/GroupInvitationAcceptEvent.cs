using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Groups.Args
{
    public class GroupInvitationAcceptEvent : ChickenEventArgs
    {
        public GroupInvitDto Invitation { get; set; }
    }
}