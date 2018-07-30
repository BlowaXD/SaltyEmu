using System;
using System.Collections.Generic;
using ChickenAPI.Data.TransferObjects.Map;
using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Components;
using ChickenAPI.Utils;

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
                { typeof(NpcMonsterComponent), new NpcMonsterComponent(this, dto) }
            };
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public MovableComponent Movable { get; }
        public BattleComponent Battle { get; set; }
    }
}