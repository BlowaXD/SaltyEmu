using ChickenAPI.Core.ECS.Systems;

namespace ChickenAPI.Game.Features.Visibility.Args
{
    public class VisibilitySetInvisibleEventArgs : SystemEventArgs
    {
        // don't know what to do in yet
        public bool Broadcast { get; set; }
        public bool IsChangingMapLayer { get; set; }
    }
}