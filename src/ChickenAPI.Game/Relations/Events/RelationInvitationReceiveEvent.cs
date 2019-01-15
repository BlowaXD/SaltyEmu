using ChickenAPI.Data.Relations;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Relations.Events
{
    public class RelationInvitationReceiveEvent : GameEntityEvent
    {
        public RelationInvitationDto Invitation { get; set; }
    }
}