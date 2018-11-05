using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.Shop;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Features.Shops;
using ChickenAPI.Game.Features.Skills;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Visibility;

namespace ChickenAPI.Game.Entities.Npc
{
    public class NpcEntity : EntityBase, INpcEntity
    {
        public NpcEntity(MapNpcDto npc, ShopDto shop) : base(VisualType.Npc, npc.Id)
        {
            Battle = new BattleComponent(this)
            {
                Hp = npc.NpcMonster.MaxHp,
                HpMax = npc.NpcMonster.MaxHp,
                Mp = npc.NpcMonster.MaxMp,
                MpMax = npc.NpcMonster.MaxMp,
                BasicArea = npc.NpcMonster.BasicArea
            };
            Movable = new MovableComponent(this, npc.IsMoving ? npc.NpcMonster.Speed : (byte)0)
            {
                Actual = new Position<short>(npc.MapX, npc.MapY),
                Destination = new Position<short>(npc.MapX, npc.MapY),
                DirectionType = npc.Position
            };
            MapNpc = npc;

            Shop = shop != null ? new Shop(shop) : null;
            SkillComponent = new SkillComponent(this);
            _visibility = new VisibilityComponent(this);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(BattleComponent), Battle },
                { typeof(VisibilityComponent), _visibility },
                { typeof(MovableComponent), Movable },
                { typeof(NpcMonsterComponent), new NpcMonsterComponent(this, npc) },
                { typeof(SkillComponent), SkillComponent }
            };
        }

        public bool HasShop => Shop != null;
        public Shop Shop { get; set; }

        public MapNpcDto MapNpc { get; set; }
        public MovableComponent Movable { get; }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region Visibility

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

        public VisibilityComponent _visibility { get; }

        #endregion

        #region Battle


        #region Skills


        public bool HasSkill(long skillId) => SkillComponent.Skills.ContainsKey(skillId);

        public bool CanCastSkill(long skillId) => SkillComponent.CooldownsBySkillId.Any(s => s.Item2 == skillId);
        public IDictionary<long, SkillDto> Skills { get; }

        public SkillComponent SkillComponent { get; }

        #endregion
        public BattleComponent Battle { get; }


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