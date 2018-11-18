using System;

namespace ChickenAPI.Game.GuriHandling.Handling
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GuriEffectAttribute : Attribute
    {
        public GuriEffectAttribute(long effectId) => EffectId = effectId;


        public long EffectId { get; set; }
    }
}