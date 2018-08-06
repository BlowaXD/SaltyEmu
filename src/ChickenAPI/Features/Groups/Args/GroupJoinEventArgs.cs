using ChickenAPI.Core.ECS.Systems.Args;

namespace ChickenAPI.Game.Features.Groups.Args
{
    public class GroupJoinEventArgs : SystemEventArgs
    {
        public GroupComponent Group { get; set; }
    }
}