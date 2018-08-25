using System;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;

namespace ChickenAPI.Game.Data.TransferObjects.Item
{
    public class ItemInstanceDto : ISynchronizedDto, ICloneable
    {
        public ItemDto Item { get; set; }

        public long ItemId { get; set; }

        public long CharacterId { get; set; }

        public long? BoundCharacterId { get; set; }

        public short Amount { get; set; }

        public short Slot { get; set; }

        public byte Design { get; set; }

        #region Jewels

        /// <summary>
        ///     Number of inserted cellon for jewels
        /// </summary>
        public byte Cellon { get; set; }

        #endregion

        #region GlovesAndBoots

        public byte Sum { get; set; }

        #endregion

        public object Clone()
        {
            object tmp = MemberwiseClone();
            if (!(tmp is ItemInstanceDto newObject))
            {
                return null;
            }

            newObject.Id = Guid.NewGuid();
            return newObject;
        }

        public Guid Id { get; set; }

        #region WeaponsAndArmors

        public byte Rarity { get; set; }

        public InventoryType Type { get; set; }

        public byte Upgrade { get; set; }

        /// <summary>
        ///     Remaining ammo for archers primary weapon or swordsmen secondary weapon
        /// </summary>
        public byte Ammo { get; set; }

        public short CloseDefense { get; set; }

        public short RangeDefense { get; set; }

        public short MagicDefense { get; set; }

        public short CloseDodge { get; set; }

        public short RangeDodge { get; set; }

        public short MagicDodge { get; set; }

        public short DamageMinimum { get; set; }

        public short DamageMaximum { get; set; }

        public short Concentration { get; set; }

        public short HitRate { get; set; }

        public short CriticalDodge { get; set; }

        public short CriticalRate { get; set; }

        public short CriticalDamageRate { get; set; }

        #endregion

        #region SpecialistCards

        public byte Level { get; set; }

        public long Xp { get; set; }

        public byte SpecialistUpgrade { get; set; }

        public byte SpecialistUpgrade2 { get; set; }

        public byte AttackPoints { get; set; }

        public byte DefensePoints { get; set; }

        public byte ElementPoints { get; set; }

        public byte HpMpPoints { get; set; }

        #endregion

        #region Fairy

        public ElementType ElementType { get; set; }

        public short ElementRate { get; set; }

        #endregion Fairy

        #region CommonToAllWearables

        public short Hp { get; set; }

        public short Mp { get; set; }

        public short FireResistance { get; set; }

        public short FirePower { get; set; }

        public short WaterResistance { get; set; }

        public short WaterPower { get; set; }

        public short LightResistance { get; set; }

        public short LightPower { get; set; }

        public short DarkResistance { get; set; }

        public short DarkPower { get; set; }

        #endregion
    }
}