using ChickenAPI.Enums.Game.Entity;

namespace ChickenAPI.Data.NpcMonster
{
    public class NpcMonsterDto : IMappedDto
    {
        #region Properties

        /// <summary>
        ///     Also associated to "Vnum"
        /// </summary>
        public long Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        ///     Like Mandra
        /// </summary>
        public bool CantWalk { get; set; }


        public bool CanCollect { get; set; }

        public bool CantDebuff { get; set; }

        public bool CanCatch { get; set; }

        public bool CanRegenMp { get; set; }

        public bool CantVoke { get; set; }

        public bool CantTargetInfo { get; set; }

        public byte AmountRequired { get; set; }

        public byte AttackClass { get; set; }

        public byte AttackUpgrade { get; set; }

        public byte BasicArea { get; set; }

        public short BasicCooldown { get; set; }

        public byte BasicRange { get; set; }

        public short BasicSkill { get; set; }

        public short CloseDefence { get; set; }

        public short Concentrate { get; set; }

        public byte CriticalChance { get; set; }

        public short CriticalRate { get; set; }

        public short DamageMaximum { get; set; }

        public short DamageMinimum { get; set; }

        public sbyte DarkResistance { get; set; }

        public short DefenceDodge { get; set; }

        public byte DefenceUpgrade { get; set; }

        public short DistanceDefence { get; set; }

        public short DistanceDefenceDodge { get; set; }

        /// <summary>
        ///     ElementType
        /// </summary>
        public ElementType Element { get; set; }

        /// <summary>
        ///     Element % in stats
        /// </summary>
        public short ElementRate { get; set; }

        /// <summary>
        ///     Fire Resistance
        /// </summary>
        public sbyte FireResistance { get; set; }

        /// <summary>
        ///     Hero level of the monster
        /// </summary>
        public byte HeroLevel { get; set; }

        /// <summary>
        ///     Hero Xp Granted by monster on Kill
        /// </summary>
        public int HeroXp { get; set; }

        /// <summary>
        ///     Is Hostile
        /// </summary>
        public bool IsHostile { get; set; }

        /// <summary>
        ///     Is Hostile if you attack a monster of the same type
        /// </summary>
        public bool IsHostileGroup { get; set; }

        public int JobXp { get; set; }

        public byte Level { get; set; }

        public sbyte LightResistance { get; set; }

        public short MagicDefence { get; set; }

        public int MaxHp { get; set; }

        public int MaxMp { get; set; }

        public MonsterType MonsterType { get; set; }

        public bool NoAggresiveIcon { get; set; }

        public byte NoticeRange { get; set; }

        public byte Race { get; set; }

        public byte RaceType { get; set; }

        /// <summary>
        ///     In Deciseconds
        /// </summary>
        public int RespawnTime { get; set; }

        public byte Speed { get; set; }

        public short VNumRequired { get; set; }

        public sbyte WaterResistance { get; set; }

        public int Xp { get; set; }

        public bool IsPercent { get; set; }

        public int TakeDamages { get; set; }

        public int GiveDamagePercentage { get; set; }

        #endregion
    }
}