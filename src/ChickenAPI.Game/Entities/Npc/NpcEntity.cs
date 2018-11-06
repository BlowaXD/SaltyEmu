using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.Shop;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Features.Shops;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Skills;
using ChickenAPI.Game.Visibility;

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
            Hp = npc.NpcMonster.MaxHp;
            Mp = npc.NpcMonster.MaxMp;
            HpMax = npc.NpcMonster.MaxHp;
            MpMax = npc.NpcMonster.MaxMp;
            BasicArea = npc.NpcMonster.BasicArea;
            Shop = shop != null ? new Shop(shop) : null;
            SkillComponent = new SkillComponent(this);
            _visibility = new VisibilityComponent(this);
            Components = new Dictionary<Type, IComponent>
            {
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

        public int MpMax { get; set; }

        public bool IsAlive => Hp > 0;
        public bool CanAttack => true;

        public byte HpPercentage => Convert.ToByte((int)(Hp / (float)HpMax * 100));
        public byte MpPercentage => Convert.ToByte((int)(Mp / (float)MpMax * 100.0));
        public byte BasicArea { get; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int HpMax { get; set; }


        #region Movements

        public Position<short> Position { get; }
        public bool IsSitting { get; }
        public bool IsWalking { get; }
        public bool CanMove => !Movable.IsSitting;
        public bool IsStanding { get; }
        public byte Speed { get; set; }
        public DateTime LastMove { get; }
        public Position<short> Actual => Movable.Actual;
        public Position<short> Destination => Movable.Destination;

        #endregion

        #endregion
    }
}