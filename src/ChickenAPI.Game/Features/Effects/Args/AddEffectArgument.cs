using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Effects.Args
{
    public class AddEffectArgument : ChickenEventArgs
    {
        public long EffectId { get; set; }

        public long Cooldown { get; set; }
    }
}