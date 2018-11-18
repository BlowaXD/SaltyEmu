using System;
using System.Collections.Generic;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Monster.Events;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Events;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Entities.Monster
{
    public class MonsterEventHandler : EventHandlerBase
    {
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

                    break;
            }
        }
    }
}