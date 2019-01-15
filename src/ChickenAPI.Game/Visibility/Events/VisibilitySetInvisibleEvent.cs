using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Visibility.Events
{
    public class VisibilitySetInvisibleEvent : GameEntityEvent
    {
        // don't know what to do in yet
        public bool Broadcast { get; set; }
        public bool IsChangingMapLayer { get; set; }
    }
}