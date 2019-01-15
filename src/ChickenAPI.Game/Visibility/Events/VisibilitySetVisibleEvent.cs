using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Visibility.Events
{
    public class VisibilitySetVisibleEvent : GameEntityEvent
    {
        public bool Broadcast { get; set; }

        public bool IsChangingMapLayer { get; set; }
    }
}