using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Effects.Args
{
    public class AddEffectEvent : GameEntityEvent
    {
        public long EffectId { get; set; }

        public long Cooldown { get; set; }
    }
}