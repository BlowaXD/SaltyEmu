using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Buffs;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Skills;
using ChickenAPI.Game.Visibility;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Entities.Mates
{
    public class MateEntity : EntityBase, IMateEntity
    {
        public MateEntity(IPlayerEntity owner, CharacterMateDto dto, NpcMonsterDto npcMonster) : base(VisualType.Monster, dto.Id)
        {
            Owner = owner;
            Mate = dto;
            NpcMonster = npcMonster;
            Level = dto.Level;
            LevelXp = dto.Experience;
            Hp = dto.Hp;
            HpMax = dto.Hp;
            Mp = dto.Mp;
            MpMax = dto.Mp;
            IsTeamMember = dto.IsTeamMember;
            MateType = dto.MateType;
            PetId = 0;
            Movable = new MovableComponent(this, NpcMonster.Speed)
            {
                Actual = new Position<short> { X = dto.MapX, Y = dto.MapY },
                Destination = new Position<short> { X = dto.MapX, Y = dto.MapY }
            };
            _visibility = new VisibilityComponent(this);
            SkillComponent = new SkillComponent(this);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(VisibilityComponent), _visibility },
                { typeof(MovableComponent), Movable },
                { typeof(SkillComponent), SkillComponent }
            };
        }

        public IPlayerEntity Owner { get; set; }
        public NpcMonsterDto NpcMonster { get; }
        public CharacterMateDto Mate { get; }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public short MorphId { get; set; }
        public int SpCoolDown { get; set; }
        public DateTime LastMorphUtc { get; set; }
        public byte PetId { get; set; }

        #region Exp

        public byte Level
        {
            get => Mate.Level;
            set => Mate.Level = value;
        }

        public long LevelXp
        {
            get => Mate.Experience;
            set => Mate.Experience = value;
        }

        public byte HeroLevel { get; set; }
        public long HeroLevelXp { get; set; }
        public byte JobLevel { get; set; }
        public long JobLevelXp { get; set; }

        #endregion Exp

        #region Movable

        private VisibilityComponent _visibility { get; }
        public MovableComponent Movable { get; }

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

        #endregion Movable

        #region Visibility

        public bool IsVisible => _visibility.IsVisible;
        public bool IsInvisible => _visibility.IsInvisible;

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

        public VisibilityType Visibility
        {
            get => _visibility.Visibility;
            set => _visibility.Visibility = value;
        }

        public byte Size { get; set; }

        public bool IsAlive => Hp > 0;
        public bool CanAttack => true;
        public byte HpPercentage => Convert.ToByte((int)(Hp / (float)HpMax * 100));
        public byte MpPercentage => Convert.ToByte((int)(Mp / (float)MpMax * 100.0));
        public byte BasicArea { get; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int HpMax { get; set; }
        public int MpMax { get; set; }

        public bool IsTeamMember { get; set; }
        public MateType MateType { get; set; }

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

        #endregion Target

        public DateTime LastTimeKilled { get; set; }
        public DateTime LastHitReceived { get; set; }

        #region Skills

        public bool HasSkill(long skillId) => SkillComponent.Skills.ContainsKey(skillId);

        public bool CanCastSkill(long skillId) => SkillComponent.CooldownsBySkillId.Any(s => s.Item2 == skillId);

        public IDictionary<long, SkillDto> Skills => SkillComponent.Skills;

        public SkillComponent SkillComponent { get; }

        #endregion Skills

        #endregion Visibility
    }
}