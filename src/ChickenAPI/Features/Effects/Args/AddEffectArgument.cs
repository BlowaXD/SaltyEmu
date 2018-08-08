using ChickenAPI.Core.ECS.Systems.Args;

namespace ChickenAPI.Game.Features.Effects.Args
{
    public class AddEffectArgument : SystemEventArgs
    {
        public long EffectId { get; set; }

        public long Cooldown { get; set; }
    }
}