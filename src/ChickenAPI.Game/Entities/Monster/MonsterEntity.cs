﻿using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.Buffs;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Skills;
using ChickenAPI.Game.Visibility;

namespace ChickenAPI.Game.Entities.Monster
{
    public class MonsterEntity : EntityBase, IMonsterEntity
    {
        public MonsterEntity(MapMonsterDto dto) : base(VisualType.Monster, dto.Id)
        {
            Movable = new MovableComponent(this, dto.IsMoving ? dto.NpcMonster.Speed : (byte)0)
            {
                Actual = new Position<short> { X = dto.MapX, Y = dto.MapY },
                Destination = new Position<short> { X = dto.MapX, Y = dto.MapY }
            };
            Level = dto.NpcMonster.Level;
            Hp = dto.NpcMonster.MaxHp;
            HpMax = dto.NpcMonster.MaxHp;
            Mp = dto.NpcMonster.MaxMp;
            MpMax = dto.NpcMonster.MaxMp;
            BasicArea = dto.NpcMonster.BasicArea;
            SkillComponent = new SkillComponent(this);
            NpcMonster = dto.NpcMonster;
            MapMonster = dto;
            _visibility = new VisibilityComponent(this);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(VisibilityComponent), _visibility },
                { typeof(MovableComponent), Movable },
                { typeof(NpcMonsterComponent), new NpcMonsterComponent(this, dto) },
                { typeof(SkillComponent), SkillComponent }
            };

            #region Stat

            Defence = dto.NpcMonster.CloseDefence;
            DefenceDodge = dto.NpcMonster.DefenceDodge;
            DistanceDefence = dto.NpcMonster.DistanceDefence;
            DistanceDefenceDodge = dto.NpcMonster.DistanceDefenceDodge;
            MagicalDefence = dto.NpcMonster.MagicDefence;
            MinHit = dto.NpcMonster.DamageMinimum;
            MaxHit = dto.NpcMonster.DamageMaximum;
            HitRate = (byte)dto.NpcMonster.Concentrate;
            CriticalChance = dto.NpcMonster.CriticalChance;
            CriticalRate = dto.NpcMonster.CriticalRate;

            #endregion Stat
        }

        #region stat

        public int MinHit { get; set; }

        public int MaxHit { get; set; }

        public int HitRate { get; set; }
        public int CriticalChance { get; set; }
        public short CriticalRate { get; set; }
        public int DistanceCriticalChance { get; set; }
        public int DistanceCriticalRate { get; set; }
        public short WaterResistance { get; set; }
        public short FireResistance { get; set; }
        public short LightResistance { get; set; }
        public short DarkResistance { get; set; }

        public short Defence { get; set; }

        public short DefenceDodge { get; set; }

        public short DistanceDefence { get; set; }

        public short DistanceDefenceDodge { get; set; }

        public short MagicalDefence { get; set; }

        #endregion stat

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public MovableComponent Movable { get; }
        public NpcMonsterDto NpcMonster { get; }
        public MapMonsterDto MapMonster { get; }

        #region Visibility

        private VisibilityComponent _visibility { get; }

        public event EventHandlerWithoutArgs<IVisibleEntity> Invisible
        {
            add => _visibility.Invisible += value;
            remove => _visibility.Invisible -= value;
        }

        public event EventHandlerWithoutArgs<IVisibleEntity> Visible
        {
            add => _visibility.Visible += value;
            remove => _visibility.Visible -= value;
        }

        public bool IsVisible => _visibility.IsVisible;

        public bool IsInvisible => _visibility.IsInvisible;

        public VisibilityType Visibility
        {
            get => _visibility.Visibility;
            set => _visibility.Visibility = value;
        }

        #endregion Visibility

        #region Battle

        #region Skills

        public bool HasSkill(long skillId) => SkillComponent.Skills.ContainsKey(skillId);

        public bool CanCastSkill(long skillId) => SkillComponent.CooldownsBySkillId.Any(s => s.Item2 == skillId);

        public IDictionary<long, SkillDto> Skills => SkillComponent.Skills;

        public SkillComponent SkillComponent { get; }

        #endregion Skills

        public int MpMax { get; set; }
        private readonly List<BuffContainer> _buffs = new List<BuffContainer>();
        public ICollection<BuffContainer> Buffs => _buffs;
        public DateTime LastTimeKilled { get; set; }
        public DateTime LastHitReceived { get; set; }

        public bool IsAlive => Hp > 0;
        public bool CanAttack => true;

        public byte HpPercentage => Convert.ToByte((int)(Hp / (float)HpMax * 100));
        public byte MpPercentage => Convert.ToByte((int)(Mp / (float)MpMax * 100.0));
        public byte BasicArea { get; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int HpMax { get; set; }

        #region Movements

        public bool IsSitting { get; }
        public bool IsWalking { get; }
        public bool CanMove => !Movable.IsSitting;
        public bool IsStanding { get; }
        public byte Speed { get; set; }
        public DateTime LastMove { get; }
        public Position<short> Position => Movable.Actual;
        public Position<short> Destination => Movable.Destination;

        #endregion Movements

        #endregion Battle

        public byte Level { get; set; }
        public long LevelXp { get; set; }
        public byte HeroLevel { get; set; }
        public long HeroLevelXp { get; set; }
        public byte JobLevel { get; set; }
        public long JobLevelXp { get; set; }
    }
}