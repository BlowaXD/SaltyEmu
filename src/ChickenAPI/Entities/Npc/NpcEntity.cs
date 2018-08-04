using System;
using System.Collections.Generic;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using ChickenAPI.Game.Features.Shops;
using ChickenAPI.Game.Game.Components;

namespace ChickenAPI.Game.Entities.Npc
{
    public class NpcEntity : EntityBase, INpcEntity
    {
        public NpcEntity(MapNpcDto npc, ShopDto shop) : base(EntityType.Npc)
        {
            Battle = new BattleComponent(this);
            Movable = new MovableComponent(this)
            {
                Actual = new Position<short>(npc.MapX, npc.MapY),
                Destination = new Position<short>(npc.MapX, npc.MapY),
                DirectionType = npc.Position,
                Speed = npc.NpcMonster.Speed
            };
            Shop = new Shop(shop);
            Skills = new SkillsComponent(this);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(BattleComponent), Battle },
                { typeof(VisibilityComponent), new VisibilityComponent(this) },
                { typeof(MovableComponent), Movable },
                { typeof(NpcMonsterComponent), new NpcMonsterComponent(this, npc) },
                { typeof(SkillsComponent), Skills }
            };
        }

        public MapNpcDto MapNpc { get; set; }

        public Shop Shop { get; set; }
        public SkillsComponent Skills { get; }
        public BattleComponent Battle { get; }
        public MovableComponent Movable { get; }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}