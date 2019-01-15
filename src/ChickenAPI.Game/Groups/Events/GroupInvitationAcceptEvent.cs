using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Groups.Events
{
    public class GroupInvitationAcceptEvent : GameEntityEvent
    {
        public GroupInvitDto Invitation { get; set; }
    }
}