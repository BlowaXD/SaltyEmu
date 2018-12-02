using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Groups.Args
{
    public class GroupInvitationRefuseEvent : ChickenEventArgs
    {
        public GroupInvitDto Invitation { get; set; }
    }
}