using System;
using System.Collections.Generic;
using ChickenAPI.Game.Buffs;

namespace ChickenAPI.Game.Battle.Interfaces
{
    public interface IBattleCapacity
    {
        /// <summary>
        ///     Tells if the entity is alive
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        ///     Tells if the entity can attack or not
        /// </summary>
        bool CanAttack { get; }

        /// <summary>
        ///     Gives the actual Hp Percentage (0-100%)
        /// </summary>
        byte HpPercentage { get; }

        /// <summary>
        ///     Gives the actual Mp Percentage (0-100%)
        /// </summary>
        byte MpPercentage { get; }

        byte BasicArea { get; }

        int Hp { get; set; }
        int Mp { get; set; }

        int HpMax { get; set; }
        int MpMax { get; set; }

        #region Buffs

        ICollection<BuffContainer> Buffs { get; }

        #endregion

        DateTime LastTimeKilled { get; set; }
        DateTime LastHitReceived { get; set; }

        #region Stats

        short Defence { get; set; }

        short DefenceDodge { get; set; }

        short DistanceDefence { get; set; }

        short DistanceDefenceDodge { get; set; }

        short MagicalDefence { get; set; }

        int MinHit { get; set; }
        int MaxHit { get; set; }
        int HitRate { get; set; }

        int CriticalChance { get; set; }
        short CriticalRate { get; set; }

        int DistanceCriticalChance { get; set; }
        int DistanceCriticalRate { get; set; }

        short WaterResistance { get; set; }
        short FireResistance { get; set; }
        short LightResistance { get; set; }
        short DarkResistance { get; set; }

        #endregion

        #region Target

        bool HasTarget { get; }

        IBattleEntity Target { get; set; }
        DateTime LastTarget { get; }

        #endregion
    }
}