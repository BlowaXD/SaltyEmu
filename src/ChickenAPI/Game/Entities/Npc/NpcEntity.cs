using System;
using System.Collections.Generic;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.TransferObjects.Map;
using ChickenAPI.Data.TransferObjects.Shop;
using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities.Npc;

namespace ChickenAPI.Game.Game.Entities.Npc
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
            Shop = shop;
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(BattleComponent), Battle },
                { typeof(VisibilityComponent), new VisibilityComponent(this) },
                { typeof(MovableComponent), Movable },
                {
                    typeof(NpcMonsterComponent), new NpcMonsterComponent(this, npc)
                }
            };
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public MapNpcDto MapNpc { get; set; }

        public ShopDto Shop { get; set; }
        public BattleComponent Battle { get; }
        public MovableComponent Movable { get; }
    }
}