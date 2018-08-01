using ChickenAPI.ECS.Entities;
using ChickenAPI.ECS.Systems;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Skills.Args;
using ChickenAPI.Game.Game.Entities.Npc;

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

            switch (component.Entity)
            {
                case PlayerEntity player:
                    break;
                case MonsterEntity monster:
                    break;
                case NpcEntity npc:
                    break;
            }
        }
    }
}