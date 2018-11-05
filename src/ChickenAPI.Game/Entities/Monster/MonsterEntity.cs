using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Features.Skills;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Movements.Extensions;
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
            Battle = new BattleComponent(this)
            {
                Hp = dto.NpcMonster.MaxHp,
                HpMax = dto.NpcMonster.MaxHp,
                Mp = dto.NpcMonster.MaxMp,
                MpMax = dto.NpcMonster.MaxMp,
                BasicArea = dto.NpcMonster.BasicArea
            };
            SkillComponent = new SkillComponent(this);
            NpcMonster = dto.NpcMonster;
            MapMonster = dto;
            _visibility = new VisibilityComponent(this);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(VisibilityComponent), _visibility },
                { typeof(BattleComponent), Battle },
                { typeof(MovableComponent), Movable },
                { typeof(NpcMonsterComponent), new NpcMonsterComponent(this, dto) },
                { typeof(SkillComponent), SkillComponent }
            };
        }

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

        #endregion

        #region Battle

        #region Skills

        public bool HasSkill(long skillId) => SkillComponent.Skills.ContainsKey(skillId);

        public bool CanCastSkill(long skillId) => SkillComponent.CooldownsBySkillId.Any(s => s.Item2 == skillId);

        public IDictionary<long, SkillDto> Skills => SkillComponent.Skills;

        public SkillComponent SkillComponent { get; }

        #endregion

        public BattleComponent Battle { get; }


        public bool IsAlive => Hp > 0;

        public byte HpPercentage => Convert.ToByte((int)(Hp / (float)HpMax * 100));
        public byte MpPercentage => Convert.ToByte((int)(Mp / (float)MpMax * 100.0));

        public int Hp
        {
            get => Battle.Hp;
            set => Battle.Hp = value;
        }

        public int Mp
        {
            get => Battle.Mp;
            set => Battle.Mp = value;
        }

        public int HpMax
        {
            get => Battle.HpMax;
            set => Battle.HpMax = value;
        }

        public int MpMax
        {
            get => Battle.MpMax;
            set => Battle.MpMax = value;
        }

        #region Movements


        public bool CanMove => !Movable.IsSitting;
        public Position<short> Actual => Movable.Actual;
        public Position<short> Destination => Movable.Destination;
        public void SetPosition(Position<short> position)
        {
            Movable.Actual = position;
        }

        public void SetPosition(short x, short y)
        {
            Movable.Actual = new Position<short>(x, y);
        }


        #endregion

        #endregion
    }
}