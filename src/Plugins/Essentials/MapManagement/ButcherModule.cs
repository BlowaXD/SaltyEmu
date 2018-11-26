using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Battle.Extensions;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Monster;
using Qmmands;
using SaltyEmu.Commands.Checks;
using SaltyEmu.Commands.Entities;

namespace Essentials.MapManagement
{
    [Group("Butcher")]
    [Description("It's a module related to butching monsters. It requires to be a GameMaster.")]
    [RequireAuthority(AuthorityType.GameMaster)]
    public class ButcherModule : SaltyModuleBase
    {
        private readonly SkillDto _skill = new SkillDto
        {
            Id = 1114,
            Cooldown = 4,
            AttackAnimation = 11,
            Effect = 4260,
            SkillType = 1
        };

        private void KillMonster(IMonsterEntity monster)
        {
            monster.Hp = 0;
            Player.Broadcast(monster.GenerateSuPacket(new HitRequest
            {
                Sender = Player,
                Target = monster,
                HitMode = SuPacketHitMode.CriticalAttack,
                Damages = (uint)(monster.HpMax > ushort.MaxValue ? ushort.MaxValue : monster.HpMax),
                UsedSkill = _skill
            }));
        }

        [Command("*")]
        [Description("Command that teleports the given player to you")]
        [Remarks("Only the player paramter is needed")]
        public async Task<SaltyCommandResult> ButchAllMonstersAsync()
        {
            IMapLayer map = Player.CurrentMap;

            if (map == null)
            {
                return await Task.FromResult(new SaltyCommandResult(true, $"You need to be in a map"));
            }


            IEnumerable<IMonsterEntity> entities = map.GetEntitiesByType<IMonsterEntity>(VisualType.Monster);
            foreach (IMonsterEntity entity in entities)
            {
                KillMonster(entity);
            }

            return await Task.FromResult(new SaltyCommandResult(true, $"Map have been cleaned"));
        }

        [Command("radius")]
        [Description("Command that teleports the given player to you")]
        [Remarks("Only the player paramter is needed")]
        public async Task<SaltyCommandResult> ButchAllMonstersAsync(
            [Description("Radius within the monsters should be killed")]
            short radius)
        {
            IMapLayer map = Player.CurrentMap;

            if (map == null)
            {
                return await Task.FromResult(new SaltyCommandResult(true, $"You need to be in a map"));
            }


            IEnumerable<IMonsterEntity> entities = map.GetEntitiesInRange<IMonsterEntity>(Player.Position, radius).Where(s => s.Type == VisualType.Monster);
            foreach (IMonsterEntity entity in entities)
            {
                KillMonster(entity);
            }

            return await Task.FromResult(new SaltyCommandResult(true, $"All monsters within a radius of {radius} tiles"));
        }
    }
}