using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Visibility.Events
{
    public class VisibilitySetInvisibleEventArgs : ChickenEventArgs
    {
        // don't know what to do in yet
        public bool Broadcast { get; set; }
        public bool IsChangingMapLayer { get; set; }
    }
}