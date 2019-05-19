using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Skills;
using ChickenAPI.Game.Skills.Args;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.EventHandlers.Skills
{
    public class Skill_AddSkill_Handler : GenericEventPostProcessorBase<PlayerAddSkillEvent>
    {
        public Skill_AddSkill_Handler(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(PlayerAddSkillEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }
            
            if (e.Skill is null)
            {
                return; //the skill doesn't exist?
            }


            if (e.ForceChecks)
            {
                if (player.Character is null)
                {
                    return; //we need it.
                }

                if (e.Skill.CpCost > 0)
                {
                    return; //not enough cp to learn that skill.
                }

                if (e.Skill.Price > int.MaxValue - 1) //we need to get entity's gold count.
                {
                    return; //not enough gold to learn that skill.
                }

                if (e.Skill.LevelMinimum > player.JobLevel)
                {
                    return; //the joblevel of the entity isn't high enough.
                }

                if (e.Skill.Class != (byte)player.Character.Class)
                {
                    return; //the class of the entity doesn't correspond of the skill requirements.
                }

                int classLevel = 0;
                switch (player.Character.Class)
                {
                    case CharacterClassType.Adventurer:
                        classLevel = e.Skill.MinimumAdventurerLevel;
                        break;

                    case CharacterClassType.Swordman:
                        classLevel = e.Skill.MinimumSwordmanLevel;
                        break;

                    case CharacterClassType.Archer:
                        classLevel = e.Skill.MinimumArcherLevel;
                        break;

                    case CharacterClassType.Magician:
                        classLevel = e.Skill.MinimumMagicianLevel;
                        break;

                    case CharacterClassType.MartialArtist:
                        classLevel = e.Skill.MinimumWrestlerLevel;
                        break;
                }

                if (classLevel > player.Level)
                {
                    return; //the level of the entity isn't high enough.
                }
            }

            if (e.Skill.Id < 200)
            {
                foreach (SkillDto skill in player.Skills.Values)
                {
                    if (e.Skill.CastId == skill.CastId && skill.Id < 200)
                    {
                        player.Skills.Remove(skill.Id);
                    }
                }
            }
            else
            {
                if (player.Skills.ContainsKey(e.Skill.Id))
                {
                    return; //we already have that skill!
                }

                if (e.Skill.UpgradeSkill != 0) //means it's not a skill but an upgrade
                {
                    SkillDto oldUpgrade = player.Skills.Values.FirstOrDefault(
                        s => s.UpgradeSkill == e.Skill.UpgradeSkill &&
                            s.UpgradeType == e.Skill.UpgradeType &&
                            s.UpgradeSkill != 0);

                    if (!(oldUpgrade is null))
                    {
                        player.Skills.Remove(oldUpgrade.Id);
                    }
                }
            }

            if (!player.Skills.ContainsKey(e.Skill.Id))
            {
                player.Skills.Add(e.Skill.Id, e.Skill);
            }

            //todo: send different packets to add the skill.
            await player.ActualizeUiSkillList();
        }
    }
}