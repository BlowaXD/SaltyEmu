using System;
using System.Linq.Expressions;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game._ECS.Systems;

namespace ChickenAPI.Game.IAs
{
    public class RespawnSystem : SystemBase
    {
        public RespawnSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        protected override Expression<Func<IEntity, bool>> Filter => entity => entity.Type == VisualType.Monster || entity.Type == VisualType.Npc;

        protected override void Execute(IEntity entity)
        {
            if (!(entity is IBattleEntity battle))
            {
                return;
            }

            if (battle.IsAlive)
            {
                return;
            }

            switch (battle)
            {
                case INpcEntity npc:
                    if ((DateTime.Now - npc.LastTimeKilled).TotalSeconds >= npc.MapNpc.NpcMonster.RespawnTime / 10d)
                    {
                        npc.Respawn();
                    }

                    break;
                case IMonsterEntity monster:
                    if ((DateTime.Now - monster.LastTimeKilled).TotalSeconds >= monster.NpcMonster.RespawnTime / 10d)
                    {
                        monster.Respawn();
                    }

                    break;
            }
        }
    }
}