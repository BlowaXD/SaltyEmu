using System.Linq;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Battle;
using ChickenAPI.Game.Features.Leveling;
using ChickenAPI.Game.Features.Skills.Args;

namespace ChickenAPI.Game.Features.Skills
{
    public class SkillEventHandler : EventHandlerBase
    {
        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            var component = entity.GetComponent<SkillComponent>();
            if (component is null)
            {
                return;
            }

            switch (e)
            {
                case SkillCastArgs skillcast:
                    TryCastSkill(component, skillcast);
                    break;
                case PlayerAddSkillEventArgs addSkill:
                    AddSkill(entity as IPlayerEntity, addSkill, component);
                    break;
            }
        }

        public static bool TryCastSkill(SkillComponent component, SkillCastArgs e)
        {
            var battleComponent = component.Entity.GetComponent<BattleComponent>();
            if (battleComponent is null)
            {
                return false;
            }

            //todo: cooldown check

            if (e.Skill.MpCost > battleComponent.Mp)
            {
                return false; //not enough mp
            }

            if (e.Skill.HitType != e.Skill.TargetType)
            {
                return false; //todo: need to ignore this check if the skill is a buff (cf: SkillDto#SkillType)
            }

            if (e.Skill.Range != e.Skill.TargetRange)
            {
                return false; //current entity is too far from target
            }

            //todo: ensure there are no more checks to do.

            return true;
        }

        public static void AddSkill(IPlayerEntity player, PlayerAddSkillEventArgs e, SkillComponent component)
        {
            if (e.Skill is null)
            {
                return; //the skill doesn't exist?
            }

            if (e.ForceChecks)
            {
                CharacterDto character = player.Character;
                ExperienceComponent experience = player.Experience;

                if (character is null)
                {
                    return; //we need it.
                }

                if (experience is null)
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

                if (e.Skill.LevelMinimum > experience.JobLevel)
                {
                    return; //the joblevel of the entity isn't high enough.
                }

                if (e.Skill.Class != (byte)character.Class)
                {
                    return; //the class of the entity doesn't correspond of the skill requirements.
                }

                int classLevel = 0;
                switch (character.Class)
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
                    case CharacterClassType.Wrestler:
                        classLevel = e.Skill.MinimumWrestlerLevel;
                        break;
                    case CharacterClassType.Unknown:
                        classLevel = e.Skill.LevelMinimum;
                        break;
                }

                if (classLevel > experience.Level)
                {
                    return; //the level of the entity isn't high enough.
                }
            }

            if (e.Skill.Id < 200)
            {
                foreach (SkillDto skill in component.Skills.Values)
                {
                    if (e.Skill.CastId == skill.CastId && skill.Id < 200)
                    {
                        component.Skills.Remove(skill.Id);
                    }
                }
            }
            else
            {
                if (component.Skills.ContainsKey(e.Skill.Id))
                {
                    return; //we already have that skill!
                }

                if (e.Skill.UpgradeSkill != 0) //means it's not a skill but an upgrade
                {
                    SkillDto oldUpgrade = component.Skills.Values.FirstOrDefault(
                        s => s.UpgradeSkill == e.Skill.UpgradeSkill &&
                            s.UpgradeType == e.Skill.UpgradeType &&
                            s.UpgradeSkill != 0);

                    if (!(oldUpgrade is null))
                    {
                        component.Skills.Remove(oldUpgrade.Id);
                    }
                }
            }

            if (!component.Skills.ContainsKey(e.Skill.Id))
            {
                component.Skills.Add(e.Skill.Id, e.Skill);
            }

            //todo: send different packets to add the skill.
        }
    }
}