using System;

namespace ChickenAPI.Game.Inventory.ItemUsage.Handling
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UseItemEffectAttribute : Attribute
    {
        public UseItemEffectAttribute(long effectId) => EffectId = effectId;

        public long EffectId { get; set; }
    }
}