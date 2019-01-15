using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Effects.Events
{
    public class AddEffectEvent : GameEntityEvent
    {
        public long EffectId { get; set; }

        public long Cooldown { get; set; }
    }
}