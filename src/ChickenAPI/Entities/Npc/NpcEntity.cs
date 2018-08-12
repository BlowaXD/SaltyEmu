using System;
using System.Collections.Generic;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Features.Battle;
using ChickenAPI.Game.Features.Movement;
using ChickenAPI.Game.Features.Shops;
using ChickenAPI.Game.Features.Skills;
using ChickenAPI.Game.Features.Visibility;

namespace ChickenAPI.Game.Entities.Npc
{
    public class NpcEntity : EntityBase, INpcEntity
    {
        public NpcEntity(MapNpcDto npc, ShopDto shop) : base(EntityType.Npc)
        {
            Battle = new BattleComponent(this)
            {
                Hp = npc.NpcMonster.MaxHp,
                HpMax = npc.NpcMonster.MaxHp,
                Mp = npc.NpcMonster.MaxMp,
                MpMax = npc.NpcMonster.MaxMp
            };
            Movable = new MovableComponent(this, npc.IsMoving ? npc.NpcMonster.Speed : (byte)0)
            {
                Actual = new Position<short>(npc.MapX, npc.MapY),
                Destination = new Position<short>(npc.MapX, npc.MapY),
                DirectionType = npc.Position
            };
            MapNpc = npc;

            Shop = shop != null ? new Shop(shop) : null;
            Skills = new SkillComponent(this);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(BattleComponent), Battle },
                { typeof(VisibilityComponent), new VisibilityComponent(this) },
                { typeof(MovableComponent), Movable },
                { typeof(NpcMonsterComponent), new NpcMonsterComponent(this, npc) },
                { typeof(SkillComponent), Skills }
            };
        }

        public Shop Shop { get; set; }
        public SkillComponent Skills { get; }

        public MapNpcDto MapNpc { get; set; }
        public BattleComponent Battle { get; }
        public MovableComponent Movable { get; }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}