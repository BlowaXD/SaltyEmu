using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Game.Entities.Drop;
using ChickenAPI.Game.Entities.Drop.Extensions;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Monster.Events;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Events;
using ChickenAPI.Game.Maps;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Monster_DeathEvent_Handler : GenericEventPostProcessorBase<MonsterDeathEvent>
    {
        private readonly IRandomGenerator Random;
        private readonly IPathfinder PathFinder;

        public Monster_DeathEvent_Handler(IRandomGenerator random, IPathfinder pathFinder)
        {
            Random = random;
            PathFinder = pathFinder;
        }

        protected override async Task Handle(MonsterDeathEvent e, CancellationToken cancellation)
        {

            if (!(e.Sender is IMonsterEntity monster))
            {
                return;
            }

            // Clear Buff/Debuff
            // Set respawn
            if (e.Killer is IPlayerEntity player)
            {
                NpcMonsterDto npcMonster = monster.NpcMonster;
                float ExpPenality(int lvlDif) => lvlDif < 5 ? 1 : (lvlDif < 10 ? 0.9f - 0.2f * (lvlDif - 6) : lvlDif < 19 ? 0.1f : 0.05f) * (2 / 3f);
                long xp = (long)(npcMonster.Xp * ExpPenality(player.Level - npcMonster.Level));
                long jobXp = (long)(npcMonster.JobXp * ExpPenality(player.JobLevel - npcMonster.Level));
                long heroXp = (long)(npcMonster.HeroXp * ExpPenality(player.Level - npcMonster.Level));
                player.EmitEvent(new ExperienceGainEvent { Experience = xp, JobExperience = jobXp, HeroExperience = heroXp });
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
        }
    }
}