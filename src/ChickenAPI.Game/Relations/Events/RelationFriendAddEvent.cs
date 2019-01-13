using ChickenAPI.Data.Relations;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Relations.Events
{
    public class RelationFriendAddEvent : GameEntityEvent
    {
        public RelationDto Relation { get; set; }
    }
}