using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Groups.Args
{
    public class GroupJoinEvent : ChickenEventArgs
    {
        public GroupDto Group { get; set; }
    }
}