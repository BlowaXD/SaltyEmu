using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Groups.Args
{
    public class GroupInvitationRefuseEvent : GameEntityEvent
    {
        public GroupInvitDto Invitation { get; set; }
    }
}