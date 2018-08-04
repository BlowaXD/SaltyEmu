using System;
using System.Collections.Generic;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Data.TransferObjects.NpcMonster;
using ChickenAPI.Game.Game.Components;

namespace ChickenAPI.Game.Entities.Monster
{
    public class MonsterEntity : EntityBase, IMonsterEntity
    {
        public MonsterEntity(MapMonsterDto dto) : base(EntityType.Monster)
        {
            Movable = new MovableComponent(this)
            {
                Actual = new Position<short> { X = dto.MapX, Y = dto.MapY },
                Destination = new Position<short> { X = dto.MapX, Y = dto.MapY }
            };
            Battle = new BattleComponent(this)
            {
                Hp = dto.NpcMonster.MaxHp,
                HpMax = dto.NpcMonster.MaxHp,
                Mp = dto.NpcMonster.MaxMp,
                MpMax = dto.NpcMonster.MaxMp
            };
            Skills = new SkillsComponent(this);
            NpcMonster = dto.NpcMonster;
            MapMonster = dto;
            Components = new Dictionary<Type, IComponent>
            {
                {
                    typeof(VisibilityComponent), new VisibilityComponent(this)
                    {
                        IsVisible = true
                    }
                },
                { typeof(BattleComponent), Battle },
                { typeof(MovableComponent), Movable },
                { typeof(NpcMonsterComponent), new NpcMonsterComponent(this, dto) },
                { typeof(SkillsComponent), Skills }
            };
        }

        public SkillsComponent Skills { get; }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public MovableComponent Movable { get; }
        public BattleComponent Battle { get; set; }
        public NpcMonsterDto NpcMonster { get; }
        public MapMonsterDto MapMonster { get; }
    }
}