using System;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;

namespace ChickenAPI.Game.Builders
{
    public class WeaponOptionBuilder
    {
        private WeaponOptionLevelType _level;
        private WeaponOptionType _option;
        private long _value;
        private Guid _weaponId;

        /// <summary>
        ///     Binds builded Equipment Option to the given item
        /// </summary>
        /// <param name="itemInstance"></param>
        public void ForWeapon(ItemInstanceDto itemInstance)
        {
            if (itemInstance.Item.EquipmentSlot != EquipmentType.MainWeapon && itemInstance.Item.EquipmentSlot != EquipmentType.SecondaryWeapon)
            {
                throw new ArgumentException("Given item should only be a weapon");
            }

            _weaponId = itemInstance.Id;
        }

        /// <summary>
        ///     Sets the level of the builded Equipment Option to the given one
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public WeaponOptionBuilder WithLevel(WeaponOptionLevelType level)
        {
            _level = level;
            return this;
        }

        /// <summary>
        ///     Sets the option of the builded Equipment Option to the given one
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public WeaponOptionBuilder WithOption(WeaponOptionType option)
        {
            _option = option;
            return this;
        }

        /// <summary>
        ///     Sets the value of the builded Equipment Option to the given one
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WeaponOptionBuilder WithValue(long value)
        {
            _value = value;
            return this;
        }


        public EquipmentOptionDto Build() =>
            new EquipmentOptionDto
            {
                Id = Guid.NewGuid(),
                WearableInstanceId = _weaponId,
                Value = _value,
                Level = (byte)_level,
                Type = (byte)_option
            };

        public static implicit operator EquipmentOptionDto(WeaponOptionBuilder builder) => builder.Build();
    }
}