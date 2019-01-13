using System;
using ChickenAPI.Enums.Game.Items;

namespace ChickenAPI.Game.Inventory.ItemUsage.Handling
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UseItemEffectAttribute : Attribute
    {
        public UseItemEffectAttribute(long effectId, ItemType itype)
        {
            EffectId = effectId;
            IType = itype;
        }

        public long EffectId { get; set; }

        public ItemType IType { get; }
    }
}