using ChickenAPI.Core.ECS.Systems.Args;

namespace ChickenAPI.Game.Features.Visibility.Args
{
    public class VisibilitySetVisibleEventArgs : SystemEventArgs
    {
        public bool Broadcast { get; set; }

        public bool IsChangingMapLayer { get; set; }
    }
}