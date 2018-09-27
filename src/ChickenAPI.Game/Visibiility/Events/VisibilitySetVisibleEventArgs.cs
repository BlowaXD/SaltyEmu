using ChickenAPI.Core.Events;

namespace ChickenAPI.Game.Features.Visibility.Args
{
    public class VisibilitySetVisibleEventArgs : ChickenEventArgs
    {
        public bool Broadcast { get; set; }

        public bool IsChangingMapLayer { get; set; }
    }
}