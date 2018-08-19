using System;

namespace ChickenAPI.Game.Features.GuriHandling
{
    public class GuriEffectAttribute : Attribute
    {
        public GuriEffectAttribute(long effectId)
        {
            EffectId = effectId;
        }


        public long EffectId { get; set; }
    }
}