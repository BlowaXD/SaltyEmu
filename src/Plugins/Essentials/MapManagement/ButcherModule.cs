using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Battle.Extensions;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities.Monster;
using Qmmands;
using SaltyEmu.Commands.Checks;
using SaltyEmu.Commands.Entities;

namespace Essentials.MapManagement
{
    [Name("Butcher")]
    [Group("Butcher")]
    [Description("Module related to monsters butching. It requires to be a GameMaster and in a Map.")]
    [RequireAuthority(AuthorityType.GameMaster)]
    [PlayerInMap]
    public class ButcherModule : SaltyModuleBase
    {
        private static readonly SkillDto Skill = new SkillDto
        {
            Id = 1114,
            Cooldown = 4,
            AttackAnimation = 11,
            Effect = 4260,
            SkillType = 1
        };

        private Task KillMonster(IBattleEntity monster)
        {
            monster.Hp = 0;
            return Context.Player.BroadcastAsync(monster.GenerateSuPacket(new HitRequest
            {
                Sender = Context.Player,
                Target = monster,
                HitMode = SuPacketHitMode.CriticalAttack,
                Damages = (uint)(monster.HpMax > ushort.MaxValue ? ushort.MaxValue : monster.HpMax),
                UsedSkill = Skill
            }));
        }

        [Command("*")]
        [Description("Kills every monster in the current map.")]
        public Task<SaltyCommandResult> ButchAllMonstersAsync()
        {
            IEnumerable<IMonsterEntity> entities = Context.Player.CurrentMap.GetEntitiesByType<IMonsterEntity>(VisualType.Monster);
            foreach (IMonsterEntity entity in entities)
            {
                KillMonster(entity);
            }

            return Task.FromResult(new SaltyCommandResult(true, $"Map have been cleaned from monsters."));
        }

        [Command("radius")]
        [Description("Kills every monster in the current map around you in the specified radius.")]
        public async Task<SaltyCommandResult> ButchAllMonstersAsync(
            [Description("Radius within the monsters should be killed.")]
            short radius)
        {
            IEnumerable<IMonsterEntity> entities = Context.Player.CurrentMap
                .GetEntitiesInRange<IMonsterEntity>(Context.Player.Position, radius)
                .Where(s => s.Type == VisualType.Monster);

            List<Task> tasks = entities.Select(KillMonster).ToList();

            await Task.WhenAll(tasks);
            return new SaltyCommandResult(true, $"All monsters within a radius of {radius} tiles have been killed.");
        }
    }
}