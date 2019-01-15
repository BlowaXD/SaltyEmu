using ChickenAPI.Enums.Game.Relations;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Relations.Events
{
    public class RelationInvitationProcessEvent : GameEntityEvent
    {
        public RelationInvitationProcessType Type { get; set; }
        public long TargetCharacterId { get; set; }
    }
}