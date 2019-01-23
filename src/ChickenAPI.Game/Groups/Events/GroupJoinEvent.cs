using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Groups.Events
{
    public class GroupJoinEvent : GameEntityEvent
    {
        public Group Group { get; set; }
    }
}