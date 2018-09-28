using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Visibility.Events
{
    public class VisibilitySetVisibleEventArgs : ChickenEventArgs
    {
        public bool Broadcast { get; set; }

        public bool IsChangingMapLayer { get; set; }
    }
}