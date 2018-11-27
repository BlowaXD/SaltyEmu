using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Drop;
using ChickenAPI.Game.Entities.Drop.Extensions;
using ChickenAPI.Game.Entities.Monster.Events;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Events;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Maps;

namespace ChickenAPI.Game.Entities.Monster
{
    public class MonsterEventHandler : EventHandlerBase
    {
        private static IRandomGenerator Random => new Lazy<IRandomGenerator>(() => ChickenContainer.Instance.Resolve<IRandomGenerator>()).Value;
        private static IPathfinder PathFinder => new Lazy<IPathfinder>(() => ChickenContainer.Instance.Resolve<IPathfinder>()).Value;

        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(MonsterDeathEvent)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case MonsterDeathEvent death:
                    if (!(entity is IMonsterEntity monster))
                    {
                        return;
                    }

                    // Clear Buff/Debuff
                    // Set respawn
                    if (death.Killer is IPlayerEntity player)
                    {
                        var npcMonster = monster.NpcMonster;
                        float ExpPenality(int lvlDif) => lvlDif < 5 ? 1 : (lvlDif < 10 ? 0.9f - 0.2f * (lvlDif - 6) : lvlDif < 19 ? 0.1f : 0.05f) * (2 / 3f);
                        long xp = (long) (npcMonster.Xp * ExpPenality(player.Level - npcMonster.Level));
                        long jobXp = (long) (npcMonster.JobXp * ExpPenality(player.JobLevel - npcMonster.Level));
                        long heroXp = (long) (npcMonster.HeroXp * ExpPenality(player.Level - npcMonster.Level));
                        player.EmitEvent(new ExperienceGainEvent {Experience = xp, JobExperience = jobXp, HeroExperience = heroXp});
                    }

                    if (Random.Next(100) < 100) // 100 should be modified with GoldDropRate
                    {
                        Position<short>[] pos = PathFinder.GetNeighbors(monster.Position, monster.CurrentMap.Map);
                        IDropEntity drop = new ItemDropEntity(monster.CurrentMap.GetNextId())
                        {
                            ItemVnum = 1046,
                            IsGold = true,
                            DroppedTimeUtc = DateTime.Now,
                            Position = pos.Length > 1 ? pos[Random.Next(pos.Length)] : monster.Position,
                            Quantity = Random.Next(6 * monster.NpcMonster.Level, 12 * monster.NpcMonster.Level)
                        };
                        drop.TransferEntity(monster.CurrentMap);
                        monster.CurrentMap.Broadcast(drop.GenerateDropPacket());
                    }
                    break;
            }
        }
    }
}