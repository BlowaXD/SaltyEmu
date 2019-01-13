using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Groups.Args
{
    public class GroupJoinEvent : GameEntityEvent
    {
        public GroupDto Group { get; set; }
    }
}