using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Groups.Args
{
    public class GroupJoinEventArgs : ChickenEventArgs
    {
        public GroupComponent Group { get; set; }
    }
}