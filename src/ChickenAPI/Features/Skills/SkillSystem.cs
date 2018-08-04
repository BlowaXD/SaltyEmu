using System.Linq;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using ChickenAPI.Game.Features.Skills.Args;

namespace ChickenAPI.Game.Features.Skills
{
    public class SkillSystem : NotifiableSystemBase
    {
        public SkillSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            var component = entity.GetComponent<SkillComponent>();
            if (component is null)
            {
                return;
            }

            switch (e)
            {
                case SkillCastArgs skillcast:
                    break;
                case PlayerAddSkillEventArgs addSkill:
                    AddSkill(component, addSkill);
                    break;
            }
        }

        public static void AddSkill(SkillComponent component, PlayerAddSkillEventArgs e)
        {
            if (e.Skill is null)
            {
                return; //the skill doesn't exist?
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