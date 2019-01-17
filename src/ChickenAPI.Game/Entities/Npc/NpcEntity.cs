﻿using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Data.Shop;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Buffs;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Shops;
using ChickenAPI.Game.Skills;
using ChickenAPI.Game.Visibility;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Entities.Npc
{
    public class NpcEntity : EntityBase, INpcEntity
    {
        public NpcEntity(MapNpcDto npc, ShopDto shop) : base(VisualType.Npc, npc.Id)
        {
            Movable = new MovableComponent(this, npc.IsMoving ? npc.NpcMonster.Speed : (byte)0)
            {
                Actual = new Position<short>(npc.MapX, npc.MapY),
                Destination = new Position<short>(npc.MapX, npc.MapY),
                DirectionType = npc.Position
            };
            MapNpc = npc;
            Level = npc.NpcMonster.Level;
            Hp = npc.NpcMonster.MaxHp;
            Mp = npc.NpcMonster.MaxMp;
            HpMax = npc.NpcMonster.MaxHp;
            MpMax = npc.NpcMonster.MaxMp;
            BasicArea = npc.NpcMonster.BasicArea;
            Shop = shop != null ? new Shop(shop, this) : null;
            SkillComponent = new SkillComponent(this);
            _visibility = new VisibilityComponent(this);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(VisibilityComponent), _visibility },
                { typeof(MovableComponent), Movable },
                { typeof(NpcMonsterComponent), new NpcMonsterComponent(this, npc) },
                { typeof(SkillComponent), SkillComponent }
            };

            #region Stat

            Defence = npc.NpcMonster.CloseDefence;
            DefenceDodge = npc.NpcMonster.DefenceDodge;
            DistanceDefence = npc.NpcMonster.DistanceDefence;
            DistanceDefenceDodge = npc.NpcMonster.DistanceDefenceDodge;
            MagicalDefence = npc.NpcMonster.MagicDefence;
            MinHit = npc.NpcMonster.DamageMinimum;
            MaxHit = npc.NpcMonster.DamageMaximum;
            HitRate = (byte)npc.NpcMonster.Concentrate;

            #endregion Stat
        }

        public MovableComponent Movable { get; }

        public bool HasShop => Shop != null;
        public Shop Shop { get; set; }

        public MapNpcDto MapNpc { get; set; }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public byte Level { get; set; }
        public long LevelXp { get; set; }
        public byte HeroLevel { get; set; }
        public long HeroLevelXp { get; set; }
        public byte JobLevel { get; set; }
        public long JobLevelXp { get; set; }
        public NpcMonsterDto NpcMonster => MapNpc.NpcMonster;

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

        #region Visibility
        
        public bool IsVisible => _visibility.IsVisible;

        public bool IsInvisible => _visibility.IsInvisible;

        public VisibilityType Visibility
        {
            get => _visibility.Visibility;
            set => _visibility.Visibility = value;
        }

        public byte Size { get; set; }

        public VisibilityComponent _visibility { get; }

        #endregion Visibility

        #region Battle

        #region Skills

        public bool HasSkill(long skillId) => SkillComponent.Skills.ContainsKey(skillId);

        public bool CanCastSkill(long skillId) => SkillComponent.CooldownsBySkillId.Any(s => s.Item2 == skillId);

        public IDictionary<long, SkillDto> Skills { get; }

        public SkillComponent SkillComponent { get; }

        #endregion Skills

        public int MpMax { get; set; }
        private readonly List<BuffContainer> _buffs = new List<BuffContainer>();
        public ICollection<BuffContainer> Buffs => _buffs;

        #region Target

        private IBattleEntity _target;
        public bool HasTarget => Target != null;

        public IBattleEntity Target
        {
            get => _target;
            set
            {
                _target = value;
                LastTarget = DateTime.Now;
            }
        }

        public DateTime LastTarget { get; private set; }

        #endregion

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

        public DirectionType DirectionType => Movable.DirectionType;

        public bool IsSitting
        {
            get => Movable.IsSitting;
            set => Movable.IsSitting = value;
        }

        public bool IsWalking => !Movable.IsSitting;
        public bool CanMove => !Movable.IsSitting;
        public bool IsStanding => !IsSitting;
        public byte Speed { get; set; }
        public DateTime LastMove { get; set; }

        public Position<short> Position
        {
            get => Movable.Actual;
            set => Movable.Actual = value;
        }

        public Position<short> Destination => Movable.Destination;

        #endregion Movements

        #endregion Battle
    }
}