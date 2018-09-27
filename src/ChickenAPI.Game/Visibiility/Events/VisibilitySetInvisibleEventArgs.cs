using ChickenAPI.Core.Events;

namespace ChickenAPI.Game.Features.Visibility.Args
{
    public class VisibilitySetInvisibleEventArgs : ChickenEventArgs
    {
        // don't know what to do in yet
        public bool Broadcast { get; set; }
        public bool IsChangingMapLayer { get; set; }
    }
}